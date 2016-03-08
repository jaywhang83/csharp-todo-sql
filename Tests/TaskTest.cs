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
      Task firstTask = new Task("Mow the lawn", testDate, false);
      Task secondTask = new Task("Mow the lawn", testDate, false);

      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save()
    {
      DateTime testDate = new DateTime(2016, 3, 1);
      Task testTask = new Task("Mow the lawn", testDate, false);
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
      Task testTask = new Task("Mow the lawn", testDate, false);

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
      Task testTask = new Task("Mow the lawn", testDate, false);
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

      Task testTask = new Task("Mow the lawn", testDate, false);
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

      Task testTask = new Task("Mow the lawn", testDate, false);
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

    [Fact]
    public void Test_GetCompletedTasks_ReturnsCompletedTasks()
    {
      DateTime testDate = new DateTime(2016, 3, 1);

      Task testTask1 = new Task("Mow the lawn", testDate, false);
      testTask1.Save();

      Task testTask2 = new Task("Walk the dog", testDate, true);
      testTask2.Save();

      List<Task> result = Task.GetCompletedTasks();
      List<Task> testList = new List<Task> {testTask2};
    }

    [Fact]
    public void Test_Delete_DeletesTaskAssociationsFromDatabase()
    {
      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      string testDescription = "Mow the lawn";
      DateTime testDate = new DateTime(2016, 3, 1);
      Task testTask = new Task(testDescription, testDate, false);
      testTask.Save();

      testTask.AddCategory(testCategory);
      testTask.Delete();

      List<Task> resultCategoryTasks = testCategory.GetTasks();
      List<Task> testCategoryList = new List<Task> {};

      Assert.Equal(resultCategoryTasks, testCategoryList);
    }

    [Fact]
    public void Test_Update_UpdatesTask()
    {
      string testDescription = "Mow the lawn";
      DateTime testDate = new DateTime(2016, 3, 1);
      Task testTask = new Task(testDescription, testDate, false);
      testTask.Save();

      string newDescription = "Wash the car";
      DateTime newDueDate = new DateTime(2016, 4, 5);
      bool taskIsCompleted = true;

      testTask.Update(newDescription, newDueDate, taskIsCompleted);

      string resultDescription = testTask.GetDescription();
      DateTime resultDueDate = testTask.GetDueDate();
      bool resultTaskCompleted = testTask.GetIsDone();

      Assert.Equal(newDescription, resultDescription);
      Assert.Equal(newDueDate, resultDueDate);
      Assert.Equal(taskIsCompleted, resultTaskCompleted);
    }

    [Fact]
    public void Test_DeleteCompletedTask_DeletesCompletedTasks()
    {
      string testDescription = "Mow the lawn";
      DateTime testDate = new DateTime(2016, 3, 1);
      Task testTask1 = new Task(testDescription, testDate, false);
      testTask1.Save();

      string testDescription2 = "Wash the car";
      DateTime testDueDate2 = new DateTime(2016, 4, 5);
      bool taskIsCompleted2 = true;
      Task testTask2 = new Task(testDescription2, testDueDate2, taskIsCompleted2);
      testTask2.Save();

      Task.DeleteCompletedTask();

      List<Task> resultList = Task.GetAll();
      List<Task> testList = new List<Task> {testTask1};

      Assert.Equal(testList, resultList);
    }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }
  }
}
