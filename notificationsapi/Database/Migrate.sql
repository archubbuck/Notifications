USE [master]
GO
/****** Object:  Database [Aspen]    Script Date: 12/4/2022 10:09:31 PM ******/
CREATE DATABASE [Aspen]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Aspen', FILENAME = N'C:\Users\Adam\Aspen.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Aspen_log', FILENAME = N'C:\Users\Adam\Aspen_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Aspen] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Aspen].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Aspen] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Aspen] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Aspen] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Aspen] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Aspen] SET ARITHABORT OFF 
GO
ALTER DATABASE [Aspen] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Aspen] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Aspen] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Aspen] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Aspen] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Aspen] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Aspen] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Aspen] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Aspen] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Aspen] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Aspen] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Aspen] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Aspen] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Aspen] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Aspen] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Aspen] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Aspen] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Aspen] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Aspen] SET  MULTI_USER 
GO
ALTER DATABASE [Aspen] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Aspen] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Aspen] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Aspen] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Aspen] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Aspen] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Aspen] SET QUERY_STORE = OFF
GO
USE [Aspen]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 12/4/2022 10:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubscriberId] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifyBy] [nvarchar](255) NULL,
	[ModifyDate] [datetimeoffset](7) NULL,
	[StopDate] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscriber]    Script Date: 12/4/2022 10:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscriber](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubscriptionId] [int] NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifyBy] [nvarchar](255) NULL,
	[ModifyDate] [datetimeoffset](7) NULL,
	[StopDate] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_Subscriber] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscription]    Script Date: 12/4/2022 10:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscription](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Platform] [varchar](16) NOT NULL,
	[Topic] [nvarchar](255) NOT NULL,
	[Template] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifyBy] [nvarchar](255) NULL,
	[ModifyDate] [datetimeoffset](7) NULL,
	[StopDate] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_Subscription] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Notification] ADD  CONSTRAINT [DF_Notification_CreatedDate]  DEFAULT (sysdatetimeoffset()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Subscriber] ADD  CONSTRAINT [DF_Subscriber_CreatedDate]  DEFAULT (sysdatetimeoffset()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Subscription] ADD  CONSTRAINT [DF_Subscription_CreatedDate]  DEFAULT (sysdatetimeoffset()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Subscriber] FOREIGN KEY([SubscriberId])
REFERENCES [dbo].[Subscriber] ([Id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Subscriber]
GO
ALTER TABLE [dbo].[Subscriber]  WITH CHECK ADD  CONSTRAINT [FK_Subscriber_Subscription] FOREIGN KEY([SubscriptionId])
REFERENCES [dbo].[Subscription] ([Id])
GO
ALTER TABLE [dbo].[Subscriber] CHECK CONSTRAINT [FK_Subscriber_Subscription]
GO
USE [master]
GO
ALTER DATABASE [Aspen] SET  READ_WRITE 
GO
