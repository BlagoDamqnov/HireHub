using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireHub.Web.Services.Data.Interfaces;
using HireHub.Data;
using HireHub.Web.Services.Data;
using HireHub.Web.ViewModels.Categories;
using HireHub.Data.Entities;

namespace HireHub.Web.Tests
{
    public class CategoryServiceTests
    {
        private ApplicationDbContext _context;
        private CategoryService _categoryService;

        [SetUp]
        public void SetUp()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(contextOptions);
            _categoryService = new CategoryService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            this._context.Database.EnsureDeleted();
        }

        [Test]
        public async Task Create_ValidData_CreatesCategory()
        {
            // Arrange
            var model = new CreateVM
            {
                CategoryName = "TestCategory"
            };

            // Act
            await _categoryService.Create(model);

            // Assert
            var createdCategory = _context.Categories.FirstOrDefault();
            Assert.IsNotNull(createdCategory);
            Assert.That(createdCategory!.CategoryName, Is.EqualTo(model.CategoryName));
        }

        [Test]
        public async Task Create_CategoryWithSameNameExists_ThrowsInvalidOperationException()
        {
            // Arrange
            var existingCategory = new Category
            {
                CategoryName = "ExistingCategory"
            };
            _context.Categories.Add(existingCategory);
            await _context.SaveChangesAsync();

            var model = new CreateVM
            {
                CategoryName = existingCategory.CategoryName // Using the same name as existing category
            };

            // Act and Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _categoryService.Create(model));
        }

        [Test]
        public async Task GetAllCategoryNames_ReturnsListOfCategoryNames()
        {
            // Arrange
            var model = new CreateVM
            {
                CategoryName = "TestCategory"
            };

            // Act
            await _categoryService.Create(model);

            // Assert
            var categoryNames = await _categoryService.GetAllCategoryNames();

            Assert.IsNotNull(categoryNames);
            Assert.That(categoryNames.Count(), Is.EqualTo(1));
            Assert.That(categoryNames.First(), Is.EqualTo(model.CategoryName));
        }

        [Test]
        public async Task IsExist_ExistingCategoryId_ReturnsTrue()
        {
            // Arrange
            var categoryId = 1;
            _context.Categories.Add(new Category { Id = categoryId,CategoryName= "TestCategoryName"});
            await _context.SaveChangesAsync();

            // Act
            var result = await _categoryService.IsExist(categoryId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsExist_NonExistingCategoryId_ReturnsFalse()
        {
            // Arrange
            var categoryId = 1;

            // Act
            var result = await _categoryService.IsExist(categoryId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
