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
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      Task firstTask = new Task("Mow the lawn");
      Task secondTask = new Task("Mow the lawn");

      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_SavesToDataBase()
    {
      Task testTask = new Task("Mow the lawn");

      testTask.Save();
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn");

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
      Task testTask = new Task("Mow the lawn");
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, foundTask);
    }

    public void Dispose()
    {
      Task.DeleteAll();
    }
  }
}
