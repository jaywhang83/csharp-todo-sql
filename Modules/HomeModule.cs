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
        List<Category> allCategories = Category.GetAll();
        return View["index.cshtml", allCategories];
      };
      Get["tasks"] = _ =>
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
        List<Category> allCategories = Category.GetAll();
        return View["tasks_form.cshtml", allCategories];
      };
      Post["/tasks/new"] = _ =>
      {
        DateTime testDate = DateTime.Parse( Request.Form["task-dueDate"]);
        Task newTask = new Task(Request.Form["task-description"], Request.Form["category-id"],testDate);
        newTask.Save();
        return View["success.cshtml"];
      };
      Post["/tasks/delete"] = _ =>
      {
        Task.DeleteAll();
        return View["cleared.cshtml"];
      };
      Get["/categories/{id}"]= parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var selectedCategory = Category.Find(parameters.id);
        var categoryTasks = selectedCategory.GetTasks();
        model.Add("category", selectedCategory);
        model.Add("tasks", categoryTasks);
        return View["category.cshtml", model];
      };
      Post["/categories/delete"] = _ =>
      {
        Category.DeleteAll();
        return View["categories-cleared.cshtml"];
      };
    }
  }
}
