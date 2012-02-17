using System;
using System.Collections.Generic;
using FubuTestingSupport;
using NUnit.Framework;
using OpenQA.Selenium;
using Serenity;
using TestContext = StoryTeller.Engine.TestContext;

namespace ProductsManagement.IntegrationTests
{
    [TestFixture]
    public class HomePageTester
    {
        private TestContext _context;
        private InProcessSerenitySystem<ProductsManagementApplication> _system;
        private NavigationDriver _navigation;

        [SetUp]
        public void Setup()
        {
            _context = new TestContext();
            _system = new InProcessSerenitySystem<ProductsManagementApplication>();
            _system.Setup();
            _system.SetupEnvironment();
            _system.RegisterServices(_context);
            _navigation = _context.Retrieve<NavigationDriver>();
            _navigation.NavigateToHome();
        }

        [TearDown]
        public void TearDown()
        {
            _navigation.Driver.Close();
            _navigation.Driver.Dispose();
        }

        [Test]
        public void page_title_is_home()
        {
            _navigation.Driver.FindElement(By.TagName("h1")).Text
                .ShouldEqual(_navigation.Driver.Title)
                .ShouldEqual("Products Management :: Home");
        }


        [Test]
        public void grid_exists()
        {
            _navigation.Driver.FindElement(By.Id("gbox_grid"));
        }

        [Test]
        public void grid_has_headers()
        {
            new[] {"Id", "Name", "Description", "Quantity"}
                .Each(x =>
                {
                    var th = "grid_" + x;
                    var label = "jqgh_" + th;
                    _navigation.Driver.FindElement(By.Id(th)).FindElement(By.Id(label)).Text.ShouldEqual(x);
                });
        }


        [Test]
        public void grid_has_crud_toolbar()
        {
            var container = _navigation.Driver.FindElement(By.Id("pager_left"));
            container.FindElement(By.ClassName("ui-icon-plus"));
            container.FindElement(By.ClassName("ui-icon-pencil"));
            container.FindElement(By.ClassName("ui-icon-trash"));
        }

        [Test]
        public void grid_has_pager_toolbar()
        {
            var container = _navigation.Driver.FindElement(By.Id("pager_center"));
            container.FindElement(By.Id("first_pager"));
            container.FindElement(By.Id("prev_pager"));
            container.FindElement(By.ClassName("ui-pg-input"));
            container.FindElement(By.Id("sp_1_pager"));
            container.FindElement(By.Id("next_pager"));
            container.FindElement(By.Id("last_pager"));
        }

        [Test]
        public void user_must_select_a_row_to_edit()
        {
            var homeUrl = _navigation.Driver.Url;
            _navigation.Driver.FindElement(By.Id("pager_left")).FindElement(By.ClassName("ui-icon-pencil")).Click();
            var alert = _navigation.Driver.SwitchTo().Alert();
            alert.ShouldNotBeNull();
            alert.Text.ShouldEqual("Please, select row");
            alert.Accept();
            _navigation.Driver.Url.ShouldEqual(homeUrl);
        }

        [Test]
        public void user_must_select_a_row_to_delete()
        {
            var homeUrl = _navigation.Driver.Url;
            _navigation.Driver.FindElement(By.Id("pager_left")).FindElement(By.ClassName("ui-icon-trash")).Click();
            var alert = _navigation.Driver.SwitchTo().Alert();
            alert.ShouldNotBeNull();
            alert.Text.ShouldEqual("Please, select row");
            alert.Accept();
            _navigation.Driver.Url.ShouldEqual(homeUrl);
        }

        [Test]
        public void editing_a_row_takes_to_edit_product_page()
        {
            _navigation.Driver.FindElement(By.Id("3")).Click();
            _navigation.Driver.FindElement(By.Id("pager_left")).FindElement(By.ClassName("ui-icon-pencil")).Click();
            var editUrl = new Uri(_navigation.Driver.Url);
            editUrl.AbsolutePath.ShouldEqual("/products/edit/3");
        }

        [Test]
        public void deleting_a_row_takes_to_delete_product_page()
        {
            _navigation.Driver.FindElement(By.Id("5")).Click();
            _navigation.Driver.FindElement(By.Id("pager_left")).FindElement(By.ClassName("ui-icon-trash")).Click();
            var deleteUrl = new Uri(_navigation.Driver.Url);
            deleteUrl.AbsolutePath.ShouldEqual("/products/delete/5");
        }
    }
}
