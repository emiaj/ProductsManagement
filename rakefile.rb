require 'rubygems'
require 'albacore'
require 'rake/clean'
include FileTest

COMPILE_TARGET = (ENV['config'] || "Debug")
CLR_TOOLS_VERSION = "v4.0.30319"
WORKING_DIR = File.dirname(__FILE__)

buildsupportfiles = Dir["#{WORKING_DIR}/buildsupport/*.rb"]
raise "Run `git submodule update --init` to populate your buildsupport folder." unless buildsupportfiles.any?
buildsupportfiles.each { |ext| load ext }

tools = Dir["#{WORKING_DIR}/tools/*.rb"]
tools.each { |ext| load ext }


ARTIFACTS = "artifacts"
COMMON_ASSEMBLY_INFO = 'src/CommonAssemblyInfo.cs';
COPYRIGHT = 'Copyright 2012, Jaime Febres Velez. All rights reserved.';
PRODUCT = "ProductsManagement"
RESULTS_DIR = "results"
SRC_DIR = "src"
STAGE_DIR = "build"

# Add directories to Rake's clean task
CLEAN.include(STAGE_DIR, ARTIFACTS)

desc "Displays a list of tasks"
task :help do
  taskHash = Hash[*(`rake.bat -T`.split(/\n/).collect { |l| l.match(/rake (\S+)\s+\#\s(.+)/).to_a }.collect { |l| [l[1], l[2]] }).flatten]

  indent = " " * 26

  puts "rake #{indent}#Runs the 'default' task"

  taskHash.each_pair do |key, value|
    if key.nil?
      next
    end
    puts "rake #{key}#{indent.slice(0, indent.length - key.length)}##{value}"
  end
end

desc "Compiles, unit tests, generates the database"
task :all => [:default]

desc "**Default**, compiles and runs tests"
task :default => [:compile, :unit_test]

desc "Update the version information for the build"
assemblyinfo :version do |asm|

  tc_build_number = ENV["BUILD_NUMBER"]
  # build script interaction with teamcity
  puts "##teamcity[buildNumber '#{build_number}-#{tc_build_number}']" unless tc_build_number.nil?

  commit = `git log -1 --pretty=format:%H`
  commit = "git unavailable" if $? != 0 # $? == exit status of the last child process to finish

  asm.trademark = commit
  asm.product_name = "#{PRODUCT} #{build_number}"
  asm.description = build_number
  asm.version = build_number
  asm.file_version = build_number
  asm.custom_attributes :AssemblyInformationalVersion => build_number
  asm.copyright = COPYRIGHT
  asm.output_file = COMMON_ASSEMBLY_INFO

end

def gittag
  return @gittag if @gittag

  # needed until teamcity provides a way to pull tags
  `git fetch` if ENV["BUILD_NUMBER"]

  description = `git describe --long`.chomp # looks something like v0.1.0-63-g92228f4

  versionpart = /^v?(\d+)\.(\d+)\.(\d+)-(\d+)-/.match(description)
  @gittag = versionpart.nil? ? [0, 0, 0, 0] : versionpart[1..5]
end

def build_number
  return @build_number if @build_number

  @build_number = gittag.join('.')
end

desc "Compiles the app"
task :compile => [:clean, :restore_if_missing, :version] do
  MSBuildRunner.compile :compilemode => COMPILE_TARGET, :solutionfile => 'src/ProductsManagement.sln', :clrversion => CLR_TOOLS_VERSION
  AspNetCompilerRunner.compile :webPhysDir => "src/ProductsManagement", :webVirDir => "localhost/xyzzyplugh"

  mkdir_p STAGE_DIR

end

def copyOutputFiles(fromDir, filePattern, outDir)
  Dir.glob(File.join(fromDir, filePattern)){|file|
        copy(file, outDir) if File.file?(file)
  }
end

desc "Runs unit tests"
task :test => [:unit_test]

desc "Runs unit tests"
task :unit_test => :compile do
  # runner = NUnitRunner.new :compilemode => COMPILE_TARGET, :source => 'src', :platform => 'x86'
  # runner.executeTests []
end

desc "Target used for the CI server"
task :ci => [:default,:package]

desc "ZIPs up the build results"
zip :package do |zip|
        mkdir_p ARTIFACTS
        zip.directories_to_zip = [STAGE_DIR]
        zip.output_file = 'productsmanagement-'+ build_number + '.zip'
        zip.output_path = [ARTIFACTS]
end



