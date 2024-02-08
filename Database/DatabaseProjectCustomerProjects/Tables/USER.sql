
CREATE TABLE [dbo].[USER]
(
  [id]                    BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
  [deletedIndicator]      BIT            NOT NULL CONSTRAINT [DF_USER_deletedIndicator] DEFAULT ((0)),
  [version]               ROWVERSION     NOT NULL,

  [companyHID]            BIGINT         NOT NULL,

  [username]              NVARCHAR (50)  NOT NULL,
  [password]              NVARCHAR (40)  NULL,
  [title]                 NVARCHAR (10)  NULL,
  [firstname]             NVARCHAR (80)  NOT NULL,
  [lastname]              NVARCHAR (80)  NOT NULL,
  [email]                 NVARCHAR (150) NULL,
  [designation]           NVARCHAR (150) NULL,

  CONSTRAINT [PK_USER] PRIMARY KEY CLUSTERED ([id] ASC),
  CONSTRAINT [FK_USER_COMPANY] FOREIGN KEY ([companyHID]) REFERENCES [dbo].[COMPANY] ([id]),

)

GO

CREATE UNIQUE NONCLUSTERED INDEX UI_DBO_USER_userName
ON [dbo].[USER]([username] ASC)
INCLUDE ([id], [companyHID], [deletedIndicator])

GO


