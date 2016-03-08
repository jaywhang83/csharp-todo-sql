using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using System;

namespace ToDoList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>
      {
        return View["index.cshtml"];
      };
      Get["/tasks"] = _ =>
      {
        List<Task> allTasks = Task.GetAll();
        return View["tasks.cshtml", allTasks];
      };
      Get["/categories"] = _ =>
      {
        List<Category> allCategories = Category.GetAll();
        return View["categories.cshtml", allCategories];
      };
      Get["/categories/new"] = _ =>
      {
        return View["categories_form.cshtml"];
      };
      Post["/categories/new"] = _ =>
      {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["success.cshtml"];
      };
      Get["/tasks/new"] = _ =>
      {
        return View["tasks_form.cshtml"];
      };
      Post["/tasks/new"] = _ =>
      {
        DateTime newDueDate = DateTime.Parse( Request.Form["task-dueDate"]);
        Task newTask = new Task(Request.Form["task-description"], newDueDate);
        newTask.Save();
        return View["success.cshtml"];
      };
      Get["/categories/{id}"]= parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Category selectedCategory = Category.Find(parameters.id);
        List<Task> categoryTasks = selectedCategory.GetTasks();
        List<Task> allTasks = Task.GetAll();
        model.Add("category", selectedCategory);
        model.Add("categoryTasks", categoryTasks);
        model.Add("alltasks", allTasks);
        return View["category.cshtml", model];
      };
      Get["/tasks/{id}"]= parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Task selectedTask = Task.Find(parameters.id);
        List<Category> taskCategories = selectedTask.GetCategories();
        List<Category> allCategories = Category.GetAll();
        model.Add("task", selectedTask);
        model.Add("taskCategories", taskCategories);
        model.Add("allCategories", allCategories);
        return View["task.cshtml", model];
      };
      Post["task/add_category"] = _ =>
      {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        task.AddCategory(category);
        return View["success.cshtml"];
      };
      Post["category/add_task"] = _ =>
      {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        category.AddTask(task);
        return View["success.cshtml"];
      };
      Post["/tasks/delete"] = _ =>
      {
        Task.DeleteAll();
        return View["cleared.cshtml"];
      };
      Post["/categories/delete"] = _ =>
      {
        Category.DeleteAll();
        return View["categories-cleared.cshtml"];
      };
      Get["category/edit/{id}"] = parameters =>
      {
        Category selectedCategory = Category.Find(parameters.id);
        return View["category_edit.cshtml", selectedCategory];
      };
      Patch["category/edit/{id}"] = parameters =>
      {
        Category selectedCategory = Category.Find(parameters.id);
        selectedCategory.Update(Request.Form["category-name"]);
        return View["success.cshtml"];
      };
      Get["category/delete/{id}"] = parameters =>
      {
        Category selectedCategory = Category.Find(parameters.id);
        return View["category_delete.cshtml", selectedCategory];
      };
      Delete["category/delete/{id}"] = parameters =>
      {
        Category selectedCategory = Category.Find(parameters.id);
        selectedCategory.Delete();
        return View["success.cshtml"];
      };
      Get["/task/edit/{id}"] = parameters =>
      {
        Task selectedTask = Task.Find(parameters.id);
        return View["task_edit.cshtml", selectedTask];
      };
      Patch["/task/edit/{id}"]= parameters =>
      {
        Task selectedTask = Task.Find(parameters.id);
        Console.WriteLine(selectedTask);
        DateTime newDueDate = DateTime.Parse( Request.Form["task-dueDate"]);
        bool taskIsCompleted = bool.Parse(Request.Form["task-completed"]);
        Console.WriteLine(taskIsCompleted);
        selectedTask.Update(Request.Form["task-description"], newDueDate, taskIsCompleted);
        return View["success.cshtml"];
      };
      Get["/task/view/completed"] = _ =>
      {
        List<Task> completedTasks = Task.GetCompletedTasks();
        foreach(var task in completedTasks)
        {
          Console.WriteLine(task.GetDescription());
        }
        return View["completedTasks.cshtml", completedTasks];
      };
    }
  }
}
