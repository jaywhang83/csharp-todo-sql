using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
        int result = Task.GetAll().Count;

        Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_overrideTrueForSameDescription()
    {
      DateTime testDate = new DateTime(2016, 3, 1);
      Task firstTask = new Task("Mow the lawn", testDate);
      Task secondTask = new Task("Mow the lawn", testDate);

      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save()
    {
      DateTime testDate = new DateTime(2016, 3, 1);
      Task testTask = new Task("Mow the lawn", testDate);
      testTask.Save();

      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      DateTime testDate = new DateTime(2016, 3, 1);
      Task testTask = new Task("Mow the lawn", testDate);

      //Act
      testTask.Save();

      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      //Arrange
      DateTime testDate = new DateTime(2016, 3, 1);
      Task testTask = new Task("Mow the lawn", testDate);
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, foundTask);
    }

    [Fact]
    public void Test_AddCategory_AddsCategoryToTask()
    {
      DateTime testDate = new DateTime(2016, 3, 1);

      Task testTask = new Task("Mow the lawn", testDate);
      testTask.Save();

      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      testTask.AddCategory(testCategory);

      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category> {testCategory};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_GetCategories_ReturnsAllCategories()
    {
      DateTime testDate = new DateTime(2016, 3, 1);

      Task testTask = new Task("Mow the lawn", testDate);
      testTask.Save();

      Category testCategory1 = new Category("Home stuff");
      testCategory1.Save();

      Category testCategory2 = new Category("Work stuff");
      testCategory2.Save();

      testTask.AddCategory(testCategory1);
      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category> {testCategory1};

      Assert.Equal(testList, result);
    }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }
  }
}
