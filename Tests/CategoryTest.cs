using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Category.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Category firstCategory = new Category("Household chores");
      Category secondCategory = new Category("Household chores");

      //Assert
      Assert.Equal(firstCategory, secondCategory);
    }

    [Fact]
    public void Test_Save_SavesCategoryToDatabase()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      //Act
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCategoryObject()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      //Act
      Category savedCategory = Category.GetAll()[0];

      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_AddTask_AddsTaskToCategory()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      DateTime testDate = new DateTime(2016, 3, 1);

      Task testTask1 = new Task("Mow the lawn", testDate, false);
      testTask1.Save();

      Task testTask2 = new Task("Water the garden", testDate, false);
      testTask2.Save();

      testCategory.AddTask(testTask1);
      testCategory.AddTask(testTask2);

      List<Task> result = testCategory.GetTasks();
      List<Task> testList = new List<Task>{testTask1, testTask2};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FiundsCategoryInDatabase()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      //Act
      Category foundCategory = Category.Find(testCategory.GetId());

      //Assert
      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void Test_GetTasks_ReturnsAllCategoryTasks()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      DateTime testDate = new DateTime(2016, 3, 1);

      Task testTask1 = new Task("Mow the lawn", testDate, false);
      testTask1.Save();

      Task testTask2 = new Task("Do the dishes", testDate, false);
      testTask2.Save();

      testCategory.AddTask(testTask1);
      List<Task> testList = new List<Task> {testTask1};
      List<Task> savedTasks = testCategory.GetTasks();

      Assert.Equal(testList, savedTasks);
    }

    [Fact]
    public void Test_Update_UpdateCategoryInDatabase()
    {
      string name = "Home stuff";
      Category testCategory = new Category(name);
      testCategory.Save();
      string newName = "Work stuff";

      testCategory.Update(newName);

      string result = testCategory.GetName();

      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_Delete_DeletesCategoryFromDatabase()
    {
      string name1 = "Home stuff";
      Category testCategory1 = new Category(name1);
      testCategory1.Save();

      string name2 = "Work stuff";
      Category testCategory2 = new Category(name2);
      testCategory2.Save();

      DateTime testDate = new DateTime(2016, 3, 2);
      Task testTask1 = new Task("Mow the lawn", testDate, false);
      testTask1.Save();
      Task testTask2 = new Task("Send emails", testDate, false);
      testTask2.Save();

      testCategory1.Delete();
      List<Category> resultCategories = Category.GetAll();
      List<Category> testCategoryList = new List<Category> {testCategory2};

      Assert.Equal(testCategoryList, resultCategories);
    }

    [Fact]
    public void Test_Delete_DeletesCategoryAssociationFromDatabase()
    {
      string testDescription = "Mow the lawn";
      DateTime testDate = new DateTime(2016, 3, 1);
      Task testTask = new Task(testDescription, testDate, false);
      testTask.Save();

      string testName = "Home stuff";
      Category testCategory = new Category(testName);
      testCategory.Save();

      testCategory.AddTask(testTask);
      testCategory.Delete();

      List<Category> resultTaskCategories = testTask.GetCategories();
      List<Category> testTaskCategories = new List<Category> {};

      Assert.Equal(resultTaskCategories, testTaskCategories);
    }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }
  }
}
