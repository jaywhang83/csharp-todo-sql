CREATE DATABASE todo;
GO
USE [todo]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 3/7/2016 4:42:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[categories_tasks]    Script Date: 3/7/2016 4:42:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories_tasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[category_id] [int] NULL,
	[task_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tasks]    Script Date: 3/7/2016 4:42:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](255) NULL,
	[dueDate] [date] NULL,
	[is_done] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[categories] ON 

INSERT [dbo].[categories] ([id], [name]) VALUES (4, N'home stuff')
INSERT [dbo].[categories] ([id], [name]) VALUES (5, N'Work related stuff')
SET IDENTITY_INSERT [dbo].[categories] OFF
SET IDENTITY_INSERT [dbo].[categories_tasks] ON 

INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (5, 4, 6)
INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (6, 4, 7)
INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (7, 5, 7)
SET IDENTITY_INSERT [dbo].[categories_tasks] OFF
SET IDENTITY_INSERT [dbo].[tasks] ON 

INSERT [dbo].[tasks] ([id], [description], [dueDate], [is_done]) VALUES (6, N'walk the dog', CAST(N'2016-03-21' AS Date), 0)
INSERT [dbo].[tasks] ([id], [description], [dueDate], [is_done]) VALUES (7, N'email person', CAST(N'2016-02-13' AS Date), 0)
SET IDENTITY_INSERT [dbo].[tasks] OFF
ALTER TABLE [dbo].[tasks] ADD  DEFAULT ((0)) FOR [is_done]
GO
