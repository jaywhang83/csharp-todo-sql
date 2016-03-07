using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList
{
  public class Task
  {
    private int Id;
    private string Description;
    private DateTime DueDate;
    private bool IsDone;

    public Task(string description, DateTime dueDate, bool isDone = false, int id = 0)
    {
      Id = id;
      Description = description;
      DueDate = dueDate;
      IsDone = isDone;
    }

    public override bool Equals(System.Object otherTask)
    {
      if (!(otherTask is Task))
      {
        return false;
      }
      else
      {
        Task newTask = (Task) otherTask;
        bool idEquality = this.GetId() == newTask.GetId();
        bool descriptionEquality = this.GetDescription() == newTask.GetDescription();
        bool dueDateEquality = this.GetDueDate() == newTask.GetDueDate();
        bool isDoneEquality = this.GetIsDone() == newTask.GetIsDone();

        return (idEquality && descriptionEquality && dueDateEquality && isDoneEquality);
      }
    }

    public int GetId()
    {
      return Id;
    }
    public string GetDescription()
    {
      return Description;
    }
    public DateTime GetDueDate()
    {
      return DueDate;
    }
    public bool GetIsDone()
    {
      return IsDone;
    }
    public void SetDescription(string newDescription)
    {
      Description = newDescription;
    }
    public void SetDueDate(DateTime newDueDate)
    {
      DueDate = newDueDate;
    }
    public void SetIsDone(bool isDone)
    {
      IsDone = isDone;
    }
    public static List<Task> GetAll()
    {
      List<Task> allTasks = new List<Task>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks ORDER BY dueDate ASC;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        DateTime taskDueDate = rdr.GetDateTime(2);
        bool taskIsDone = rdr.GetBoolean(3);
        Task newTask = new Task(taskDescription, taskDueDate, taskIsDone, taskId);
        allTasks.Add(newTask);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allTasks;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO tasks (description, dueDate, is_done) OUTPUT INSERTED.id VALUES (@TaskDescription,  @TaskDueDate, @TaskIsDone);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@TaskDescription";
      descriptionParameter.Value = this.GetDescription();

      SqlParameter dueDateParameter = new SqlParameter();
      dueDateParameter.ParameterName = "@TaskDueDate";
      dueDateParameter.Value = this.GetDueDate();

      SqlParameter isDoneParameter = new SqlParameter();
      isDoneParameter.ParameterName = "@TaskIsDone";
      isDoneParameter.Value = this.GetIsDone();

      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(dueDateParameter);
      cmd.Parameters.Add(isDoneParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId;", conn);
      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = id.ToString();
      cmd.Parameters.Add(taskIdParameter);
      rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskDescription = null;
      DateTime foundTaskDueDate = new DateTime();
      bool foundTaskIsDone = false;
      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
        foundTaskDueDate = rdr.GetDateTime(2);
        foundTaskIsDone = rdr.GetBoolean(3);
      }
      Task foundTask = new Task(foundTaskDescription, foundTaskDueDate, foundTaskIsDone, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundTask;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd= new SqlCommand("Delete FROM tasks WHERE id = @TaskId; DELETE FROM  categories_tasks WHERE task_id = @TaskId;", conn);

      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = this.GetId();

      cmd.Parameters.Add(taskIdParameter);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public void AddCategory(Category newCategory)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories_tasks (category_id, task_id) VALUES (@CategoryId, @TaskId);", conn);

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = newCategory.GetId();
      cmd.Parameters.Add(categoryIdParameter);

      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = this.GetId();
      cmd.Parameters.Add(taskIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Category> GetCategories()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT category_id FROM categories_tasks WHERE task_id = @TaskId;", conn);

      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = this.GetId();
      cmd.Parameters.Add(taskIdParameter);

      rdr = cmd.ExecuteReader();

      List<int> categoryIds = new List<int> {};

      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        categoryIds.Add(categoryId);
      }
      if(rdr != null)
      {
        rdr.Close();
      }

      List<Category>categories = new List<Category> {};

      foreach(int categoryId in categoryIds)
      {
        SqlDataReader queryReader = null;
        SqlCommand categoryQuery = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId;", conn);

        SqlParameter categoryIdParameter = new SqlParameter();
        categoryIdParameter.ParameterName = "@CategoryID";
        categoryIdParameter.Value = categoryId;
        categoryQuery.Parameters.Add(categoryIdParameter);

        queryReader = categoryQuery.ExecuteReader();
        while(queryReader.Read())
        {
          int thisCategoryId = queryReader.GetInt32(0);
          string categoryName = queryReader.GetString(1);
          Category newCategory = new Category(categoryName, thisCategoryId);
          categories.Add(newCategory);
        }
        if(queryReader != null)
        {
          queryReader.Close();
        }
      }
      if(conn != null)
      {
        conn.Close();
      }
      return categories;
    }
    public List<Task> GetCompletedTasks()
    {
      List<Task> completedTasks = new List<Task> {};
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE is_done = 1;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        DateTime taskDueDate= rdr.GetDateTime(2);
        bool taskIsDone = rdr.GetBoolean(3);
        Task newTask = new Task(taskDescription, taskDueDate, taskIsDone, taskId);
        completedTasks.Add(newTask);
      }

      if(rdr != null)
      {
        conn.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return completedTasks;
    }
  }
}
