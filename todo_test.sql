CREATE DATABASE todo_test;
GO
USE [todo_test]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 3/7/2016 4:43:10 PM ******/
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
/****** Object:  Table [dbo].[categories_tasks]    Script Date: 3/7/2016 4:43:10 PM ******/
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
/****** Object:  Table [dbo].[tasks]    Script Date: 3/7/2016 4:43:10 PM ******/
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
SET IDENTITY_INSERT [dbo].[categories_tasks] ON 

INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (5, 4, 6)
INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (6, 4, 7)
INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (7, 5, 7)
INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (8, 11, 10)
INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (10, 14, 13)
INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (11, 14, 14)
INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (12, 15, 15)
INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (13, 17, 16)
SET IDENTITY_INSERT [dbo].[categories_tasks] OFF
ALTER TABLE [dbo].[tasks] ADD  DEFAULT ((0)) FOR [is_done]
GO
