
CREATE TABLE [dbo].[PROJECT]
(
  [id]                    BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
  [deletedIndicator]      BIT            NOT NULL CONSTRAINT [DF_PROJECT_deletedIndicator] DEFAULT ((0)),
  [version]               ROWVERSION     NOT NULL,

  [companyHID]            BIGINT         NOT NULL,

  [managerHID]            BIGINT         NULL,

  [name]                  NVARCHAR(100)  NOT NULL,
  [description]           NVARCHAR(256)  NULL,
  [code]                  NVARCHAR(10)   NULL,


  CONSTRAINT [PK_PROJECT] PRIMARY KEY CLUSTERED ([id] ASC),
  CONSTRAINT [FK_PROJECT_COMPANY] FOREIGN KEY ([companyHID]) REFERENCES [dbo].[COMPANY] ([id]),
  CONSTRAINT [FK_PROJECT_USER_manager] FOREIGN KEY ([managerHID]) REFERENCES [dbo].[USER] ([id]),

)

GO



