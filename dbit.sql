Create database itlatwitter
USE [itlatwitter]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Usuarios](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nchar](50) NULL,
	[Apellido] [nchar](50) NULL,
	[Telefono] [nchar](30) NULL,
	[Correo] [nchar](50) NULL,
	[Usuario] [nchar](15) NULL,
	[Contraseña] [nchar](100) NULL,
	[Estado] [nchar](10) NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Publicaciones](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Contenido] [nchar](1000) NULL,
	[idusuario] [int] NULL,
	[Fecha] [datetime] NULL,
 CONSTRAINT [PK_Publicaciones] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Publicaciones]  WITH CHECK ADD  CONSTRAINT [FK_Publicaciones_Usuarios] FOREIGN KEY([idusuario])
REFERENCES [dbo].[Usuarios] ([id])
GO

ALTER TABLE [dbo].[Publicaciones] CHECK CONSTRAINT [FK_Publicaciones_Usuarios]
GO


CREATE TABLE [dbo].[Comentario](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idusuario] [int] NULL,
	[idpublicacion] [int] NULL,
	[contenido] [nchar](1000) NULL,
 CONSTRAINT [PK_Comentario] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Amigos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idenvia] [int] NULL,
	[idrecibe] [int] NULL,
 CONSTRAINT [PK_Amigos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Amigos]  WITH CHECK ADD  CONSTRAINT [FK_Amigos_envia] FOREIGN KEY([idenvia])
REFERENCES [dbo].[Usuarios] ([id])
GO

ALTER TABLE [dbo].[Amigos] CHECK CONSTRAINT [FK_Amigos_envia]
GO

ALTER TABLE [dbo].[Amigos]  WITH CHECK ADD  CONSTRAINT [FK_Amigos_recibe] FOREIGN KEY([idrecibe])
REFERENCES [dbo].[Usuarios] ([id])
GO

ALTER TABLE [dbo].[Amigos] CHECK CONSTRAINT [FK_Amigos_recibe]
GO



