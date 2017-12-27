USE [master]
GO
/****** Object:  Database [HardDiskManage]    Script Date: 2017/12/27 下午 09:16:28 ******/
CREATE DATABASE [HardDiskManage] ON  PRIMARY 
( NAME = N'HardDiskManage', FILENAME = N'D:\Database\HardDiskManage.mdf' , SIZE = 1406976KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HardDiskManage_log', FILENAME = N'D:\Database\HardDiskManage_log.ldf' , SIZE = 321088KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HardDiskManage] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HardDiskManage].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HardDiskManage] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HardDiskManage] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HardDiskManage] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HardDiskManage] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HardDiskManage] SET ARITHABORT OFF 
GO
ALTER DATABASE [HardDiskManage] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HardDiskManage] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HardDiskManage] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HardDiskManage] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HardDiskManage] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HardDiskManage] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HardDiskManage] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HardDiskManage] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HardDiskManage] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HardDiskManage] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HardDiskManage] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HardDiskManage] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HardDiskManage] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HardDiskManage] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HardDiskManage] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HardDiskManage] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HardDiskManage] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HardDiskManage] SET RECOVERY FULL 
GO
ALTER DATABASE [HardDiskManage] SET  MULTI_USER 
GO
ALTER DATABASE [HardDiskManage] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HardDiskManage] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'HardDiskManage', N'ON'
GO
USE [HardDiskManage]
GO
/****** Object:  Table [dbo].[KM_file]    Script Date: 2017/12/27 下午 09:16:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[KM_file](
	[SeqNo] [bigint] IDENTITY(1,1) NOT NULL,
	[FlowId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[file_path] [nvarchar](1024) NULL,
	[file_name] [nvarchar](1024) NULL,
	[file_type] [varchar](10) NOT NULL,
	[updatetime] [datetime] NOT NULL,
	[isDel] [bit] NOT NULL CONSTRAINT [DF_KM_file_isDel]  DEFAULT ((0)),
	[isVirt] [bit] NOT NULL CONSTRAINT [DF_KM_file_isVal]  DEFAULT ((0)),
	[Link] [varchar](max) NULL DEFAULT (''),
	[Metering] [bigint] NULL DEFAULT ((0)),
 CONSTRAINT [PK_KM_file] PRIMARY KEY NONCLUSTERED 
(
	[FlowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_KM_file]    Script Date: 2017/12/27 下午 09:16:29 ******/
CREATE CLUSTERED INDEX [IX_KM_file] ON [dbo].[KM_file]
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KM_file_Annotation]    Script Date: 2017/12/27 下午 09:16:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KM_file_Annotation](
	[SeqNo] [bigint] IDENTITY(1,1) NOT NULL,
	[FlowId] [uniqueidentifier] NOT NULL,
	[Annotation] [nvarchar](max) NULL,
	[updatetime] [datetime] NOT NULL,
 CONSTRAINT [PK_KM_file_Annotation] PRIMARY KEY NONCLUSTERED 
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Index [IX_KM_file_Annotation]    Script Date: 2017/12/27 下午 09:16:29 ******/
CREATE CLUSTERED INDEX [IX_KM_file_Annotation] ON [dbo].[KM_file_Annotation]
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KM_file_Comments]    Script Date: 2017/12/27 下午 09:16:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KM_file_Comments](
	[SeqNo] [bigint] IDENTITY(1,1) NOT NULL,
	[FlowId] [uniqueidentifier] NOT NULL,
	[Comments] [nvarchar](max) NULL,
	[updatetime] [datetime] NOT NULL,
	[CommentsAuthor] [nvarchar](50) NULL,
 CONSTRAINT [PK_KM_file_Comments] PRIMARY KEY NONCLUSTERED 
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Index [IX_KM_file_Comments]    Script Date: 2017/12/27 下午 09:16:29 ******/
CREATE CLUSTERED INDEX [IX_KM_file_Comments] ON [dbo].[KM_file_Comments]
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KM_file_Keywords]    Script Date: 2017/12/27 下午 09:16:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KM_file_Keywords](
	[SeqNo] [bigint] IDENTITY(1,1) NOT NULL,
	[FlowId] [uniqueidentifier] NOT NULL,
	[Keywords] [nvarchar](max) NULL,
	[isDel] [bit] NULL DEFAULT ((0)),
	[updatetime] [datetime] NULL,
	[Metering] [bigint] NULL DEFAULT ((0)),
 CONSTRAINT [PK_KM_file_Keywords] PRIMARY KEY NONCLUSTERED 
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Index [IX_KM_file_Keywords]    Script Date: 2017/12/27 下午 09:16:29 ******/
CREATE CLUSTERED INDEX [IX_KM_file_Keywords] ON [dbo].[KM_file_Keywords]
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KM_file_Temp_Path]    Script Date: 2017/12/27 下午 09:16:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KM_file_Temp_Path](
	[idx] [bigint] IDENTITY(1,1) NOT NULL,
	[file_path] [nvarchar](max) NOT NULL,
	[file_guid_path] [nvarchar](max) NOT NULL,
	[file_createtime] [datetime] NOT NULL,
 CONSTRAINT [PK_KM_file_Temp_Path] PRIMARY KEY CLUSTERED 
(
	[idx] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_KM_file_for_filename]    Script Date: 2017/12/27 下午 09:16:29 ******/
CREATE NONCLUSTERED INDEX [IX_KM_file_for_filename] ON [dbo].[KM_file]
(
	[file_path] ASC,
	[file_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[KM_file_Annotation]  WITH CHECK ADD  CONSTRAINT [FK_KM_file_Annotation_KM_file] FOREIGN KEY([FlowId])
REFERENCES [dbo].[KM_file] ([FlowId])
GO
ALTER TABLE [dbo].[KM_file_Annotation] CHECK CONSTRAINT [FK_KM_file_Annotation_KM_file]
GO
ALTER TABLE [dbo].[KM_file_Comments]  WITH CHECK ADD  CONSTRAINT [FK_KM_file_Comments_KM_file] FOREIGN KEY([FlowId])
REFERENCES [dbo].[KM_file] ([FlowId])
GO
ALTER TABLE [dbo].[KM_file_Comments] CHECK CONSTRAINT [FK_KM_file_Comments_KM_file]
GO
ALTER TABLE [dbo].[KM_file_Keywords]  WITH CHECK ADD  CONSTRAINT [FK_KM_file_Keywords_KM_file] FOREIGN KEY([FlowId])
REFERENCES [dbo].[KM_file] ([FlowId])
GO
ALTER TABLE [dbo].[KM_file_Keywords] CHECK CONSTRAINT [FK_KM_file_Keywords_KM_file]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料表索引' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KM_file', @level2type=N'COLUMN',@level2name=N'SeqNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KM_file', @level2type=N'COLUMN',@level2name=N'file_path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KM_file', @level2type=N'COLUMN',@level2name=N'file_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KM_file', @level2type=N'COLUMN',@level2name=N'file_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KM_file', @level2type=N'COLUMN',@level2name=N'updatetime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否刪除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KM_file', @level2type=N'COLUMN',@level2name=N'isDel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為虛擬路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KM_file', @level2type=N'COLUMN',@level2name=N'isVirt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'索引' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KM_file_Temp_Path', @level2type=N'COLUMN',@level2name=N'idx'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檔案路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KM_file_Temp_Path', @level2type=N'COLUMN',@level2name=N'file_path'
GO
USE [master]
GO
ALTER DATABASE [HardDiskManage] SET  READ_WRITE 
GO
