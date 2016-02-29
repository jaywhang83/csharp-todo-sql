using Nancy;
using System.Collections.Generic;
using ToDoList;
using System;

namespace ToDoList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Task> allTasks = Task.GetAll();
        return View["index.cshtml", allTasks];
      };
      Get["/tasks/new"] = _ => {
        return View["tasks_form.cshtml"];
      };
      Post["/tasks/new"] = _ => {
        Task newTask = new Task(Request.Form["new-task"]);
        newTask.Save();
        return View["task.cshtml", newTask];
      };
      Get["/tasks/delete"] = _ => {
        Task.DeleteAll();
        return View["task-deleted.cshtml"];
      };
      Get["/tasks/search"] = _ => {
        return View["task-search.cshtml"];
      };
      Post["/tasks/search/results"] = _ => {
        int id = int.Parse(Request.Form["search-task"]);
        var test = Task.Find(id);
        Console.WriteLine(test.GetDescription());
        return View["search-results.cshtml", test];
      };
    }
  }
}
