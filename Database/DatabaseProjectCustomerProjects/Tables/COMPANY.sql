
CREATE TABLE [dbo].[COMPANY]
(
  [id]                    BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
  [deletedIndicator]      BIT            NOT NULL CONSTRAINT [DF_COMPANY_deletedIndicator] DEFAULT ((0)),
  [version]               ROWVERSION     NOT NULL,


  [name]                  NVARCHAR (100) NOT NULL,
  [address1]              NVARCHAR (200) NOT NULL,
  [address2]              NVARCHAR (200) NULL,
  [town]                  NVARCHAR (200) NOT NULL,
  [postalCode]            NVARCHAR (20)  NOT NULL,
  [country]               NVARCHAR (50)  NOT NULL,


  CONSTRAINT [PK_COMPANY] PRIMARY KEY CLUSTERED ([id] ASC),

)

GO




