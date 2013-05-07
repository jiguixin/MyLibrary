USE Security
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_Right_RightRule') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE Rights DROP CONSTRAINT FK_Right_RightRule
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_RoleRight_Right') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE RoleRight DROP CONSTRAINT FK_RoleRight_Right
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_RoleRight_Role') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE RoleRight DROP CONSTRAINT FK_RoleRight_Role
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_UserRight_Rights') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE UserRight DROP CONSTRAINT FK_UserRight_Rights
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_UserRight_Users') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE UserRight DROP CONSTRAINT FK_UserRight_Users
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_UserRole_Roles') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE UserRole DROP CONSTRAINT FK_UserRole_Roles
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('FK_UserRole_Users') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE UserRole DROP CONSTRAINT FK_UserRole_Users
;



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('RightRule') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE RightRule
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('Rights') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE Rights
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('RoleRight') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE RoleRight
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('Roles') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE Roles
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('UserRight') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE UserRight
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('UserRole') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE UserRole
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('Users') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE Users
;


CREATE TABLE RightRule ( 
	RuleId nvarchar(50) NOT NULL,
	RuleType nvarchar(50) NULL,
	Assembly nvarchar(50) NULL
)
;

CREATE TABLE Rights ( 
	RightId nvarchar(50) NOT NULL,
	RightName nvarchar(50) NOT NULL,
	RightDescription nvarchar(255) NULL,
	RuleId nvarchar(50) NULL
)
;

CREATE TABLE RoleRight ( 
	RoleId nvarchar(50) NULL,
	RightId nvarchar(50) NULL
)
;

CREATE TABLE Roles ( 
	RoleId nvarchar(50) NOT NULL,
	RoleName nvarchar(50) NOT NULL,
	RoleDescription nvarchar(255) NULL
)
;

CREATE TABLE UserRight ( 
	UserId nvarchar(50) NULL,
	RightId nvarchar(50) NULL
)
;

CREATE TABLE UserRole ( 
	UserId nvarchar(50) NULL,
	RoleId nvarchar(50) NULL
)
;

CREATE TABLE Users ( 
	UserId nvarchar(50) NOT NULL,
	UserName nvarchar(50) NOT NULL,
	UserPwd nvarchar(150) NULL,
	CreateTime datetime NULL,
	LastLogin datetime NULL
)
;


ALTER TABLE RightRule ADD CONSTRAINT PK_RightRule 
	PRIMARY KEY CLUSTERED (RuleId)
;

ALTER TABLE Rights ADD CONSTRAINT PK_Right 
	PRIMARY KEY CLUSTERED (RightId)
;

ALTER TABLE Roles ADD CONSTRAINT PK_Role 
	PRIMARY KEY CLUSTERED (RoleId)
;

ALTER TABLE Users ADD CONSTRAINT PK_User 
	PRIMARY KEY CLUSTERED (UserId)
;


ALTER TABLE Rights
	ADD CONSTRAINT UQ_Right_RightName UNIQUE (RightName)
;

ALTER TABLE Users
	ADD CONSTRAINT UQ_User_UserName UNIQUE (UserName)
;


ALTER TABLE Rights ADD CONSTRAINT FK_Right_RightRule 
	FOREIGN KEY (RuleId) REFERENCES RightRule (RuleId)
;

ALTER TABLE RoleRight ADD CONSTRAINT FK_RoleRight_Right 
	FOREIGN KEY (RightId) REFERENCES Rights (RightId)
;

ALTER TABLE RoleRight ADD CONSTRAINT FK_RoleRight_Role 
	FOREIGN KEY (RoleId) REFERENCES Roles (RoleId)
;

ALTER TABLE UserRight ADD CONSTRAINT FK_UserRight_Rights 
	FOREIGN KEY (RightId) REFERENCES Rights (RightId)
;

ALTER TABLE UserRight ADD CONSTRAINT FK_UserRight_Users 
	FOREIGN KEY (UserId) REFERENCES Users (UserId)
;

ALTER TABLE UserRole ADD CONSTRAINT FK_UserRole_Roles 
	FOREIGN KEY (RoleId) REFERENCES Roles (RoleId)
;

ALTER TABLE UserRole ADD CONSTRAINT FK_UserRole_Users 
	FOREIGN KEY (UserId) REFERENCES Users (UserId)
;
