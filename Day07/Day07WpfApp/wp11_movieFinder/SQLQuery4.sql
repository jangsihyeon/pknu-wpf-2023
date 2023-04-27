
CREATE TABLE [dbo].[FavoriteMovieItem](
	[Id] [int] NOT NULL PRIMARY KEY,
	[Title] [varchar](300) NOT NULL,
	[Original_Title] [varchar](300) NOT NULL,
	[Release_Date] [char](10) NOT NULL,
	[Original_Language] [varchar](10) NOT NULL,
	[Adult] [bit] NULL,
	[Popularity] [float] NOT NULL,
	[Vote_Average] [float] NOT NULL,
	[Poster_Path] [varchar](300) NULL,
	[Overview] [ntext] NULL,
	[Reg_Date] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [pknu] SET  READ_WRITE 
GO
