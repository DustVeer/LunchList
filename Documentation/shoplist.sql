USE [dbi553714_shoplist]
GO
/****** Object:  Table [dbo].[grocery_items]    Script Date: 03/06/2025 10:58:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[grocery_items](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[quantity] [int] NULL,
	[is_checked] [tinyint] NULL,
	[retailer_product_id] [int] NULL,
 CONSTRAINT [grocery_item_pk] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[grocery_list_items]    Script Date: 03/06/2025 10:58:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[grocery_list_items](
	[grocery_list_id] [int] NOT NULL,
	[grocery_item_id] [int] NOT NULL,
 CONSTRAINT [grocery_list_items_pk] PRIMARY KEY CLUSTERED 
(
	[grocery_list_id] ASC,
	[grocery_item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[grocery_lists]    Script Date: 03/06/2025 10:58:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[grocery_lists](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NULL,
	[name] [varchar](100) NULL,
	[is_done] [tinyint] NULL,
 CONSTRAINT [grocery_lists_pk] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[retailer_products]    Script Date: 03/06/2025 10:58:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[retailer_products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NULL,
	[price] [decimal](18, 2) NULL,
	[retailer_id] [int] NULL,
 CONSTRAINT [retailer_products_pk] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[retailers]    Script Date: 03/06/2025 10:58:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[retailers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
 CONSTRAINT [retailers_pk] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[grocery_lists] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[grocery_lists] ADD  DEFAULT ((0)) FOR [is_done]
GO
ALTER TABLE [dbo].[grocery_items]  WITH CHECK ADD  CONSTRAINT [grocery_items_retailer_products_id_fk] FOREIGN KEY([retailer_product_id])
REFERENCES [dbo].[retailer_products] ([id])
GO
ALTER TABLE [dbo].[grocery_items] CHECK CONSTRAINT [grocery_items_retailer_products_id_fk]
GO
ALTER TABLE [dbo].[grocery_list_items]  WITH CHECK ADD  CONSTRAINT [grocery_list_items_grocery_items_id_fk] FOREIGN KEY([grocery_item_id])
REFERENCES [dbo].[grocery_items] ([id])
GO
ALTER TABLE [dbo].[grocery_list_items] CHECK CONSTRAINT [grocery_list_items_grocery_items_id_fk]
GO
ALTER TABLE [dbo].[grocery_list_items]  WITH CHECK ADD  CONSTRAINT [grocery_list_items_grocery_lists_id_fk] FOREIGN KEY([grocery_list_id])
REFERENCES [dbo].[grocery_lists] ([id])
GO
ALTER TABLE [dbo].[grocery_list_items] CHECK CONSTRAINT [grocery_list_items_grocery_lists_id_fk]
GO
ALTER TABLE [dbo].[retailer_products]  WITH CHECK ADD  CONSTRAINT [retailer_products_retailers_id_fk] FOREIGN KEY([retailer_id])
REFERENCES [dbo].[retailers] ([id])
GO
ALTER TABLE [dbo].[retailer_products] CHECK CONSTRAINT [retailer_products_retailers_id_fk]
GO
