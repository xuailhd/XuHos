
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for conversationfriends
-- ----------------------------
DROP TABLE IF EXISTS `conversationfriends`;
CREATE TABLE `conversationfriends`  (
  `FriendID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ConversationRoomID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `FromUserIdentifier` int(11) NOT NULL,
  `ToUserIdentifier` int(11) NOT NULL,
  `GroupName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Remark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
  `AddWording` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
  `ToUserID` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `FromUserID` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`FriendID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for conversationimuids
-- ----------------------------
DROP TABLE IF EXISTS `conversationimuids`;
CREATE TABLE `conversationimuids`  (
  `Identifier` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Enable` tinyint(1) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`Identifier`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for conversationmessages
-- ----------------------------
DROP TABLE IF EXISTS `conversationmessages`;
CREATE TABLE `conversationmessages`  (
  `ConversationMessageID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ConversationRoomID` int(11) NOT NULL,
  `ServiceID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `MessageType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `MessageContent` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `MessageTime` datetime(0) NULL,
  `MessageState` int(11) NOT NULL,
  `MessageSeq` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `MessageIndex` int(11) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`ConversationMessageID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for conversationrecordings
-- ----------------------------
DROP TABLE IF EXISTS `conversationrecordings`;
CREATE TABLE `conversationrecordings`  (
  `FileID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ChannelID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FileURL` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`FileID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for conversationroomlogs
-- ----------------------------
DROP TABLE IF EXISTS `conversationroomlogs`;
CREATE TABLE `conversationroomlogs`  (
  `ConversationRoomLogID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ConversationRoomID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OperationUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OperatorUserName` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OperatorType` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OperationTime` datetime(0) NULL,
  `OperationDesc` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OperationRemark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`ConversationRoomLogID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for conversationrooms
-- ----------------------------
DROP TABLE IF EXISTS `conversationrooms`;
CREATE TABLE `conversationrooms`  (
  `ConversationRoomID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ServiceID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ServiceType` int(11) NOT NULL,
  `ChannelID` int(11) NOT NULL,
  `Secret` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `RoomState` int(11) NOT NULL,
  `BeginTime` datetime(0) NULL,
  `EndTime` datetime(0) NULL,
  `TotalTime` int(11) NOT NULL,
  `Duration` int(11) NOT NULL,
  `Enable` tinyint(1) NOT NULL,
  `DisableWebSdkInteroperability` tinyint(1) NOT NULL,
  `Close` tinyint(1) NOT NULL,
  `RoomType` int(11) NOT NULL,
  `TriageID` bigint(20) NOT NULL,
  `Priority` int(11) NOT NULL,
  `ChargingState` int(11) NOT NULL,
  `ChargingSeq` int(11) NOT NULL,
  `ChargingTime` datetime(0) NULL,
  `ChargingInterval` int(11) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`ConversationRoomID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for conversationroomuids
-- ----------------------------
DROP TABLE IF EXISTS `conversationroomuids`;
CREATE TABLE `conversationroomuids`  (
  `ConversationRoomID` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Identifier` int(11) NOT NULL,
  `UserType` int(11) NOT NULL,
  `ChannelID` int(11) NOT NULL,
  `UserMemberID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserCNName` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserENName` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `PhotoUrl` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`ConversationRoomID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for doctormembers
-- ----------------------------
DROP TABLE IF EXISTS `doctormembers`;
CREATE TABLE `doctormembers`  (
  `DoctorMemberID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DoctorID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `MemberID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`DoctorMemberID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for doctors
-- ----------------------------
DROP TABLE IF EXISTS `doctors`;
CREATE TABLE `doctors`  (
  `DoctorID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DoctorName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Gender` int(11) NOT NULL,
  `Marriage` int(11) NOT NULL,
  `Birthday` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `IDType` int(11) NOT NULL,
  `IDNumber` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Address` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PostCode` varchar(6) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Intro` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `IsExpert` bit(1) NOT NULL,
  `Specialty` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `areaCode` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `HospitalID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `HospitalName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Grade` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DepartmentID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DepartmentName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Education` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Title` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Duties` varchar(512) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CheckState` int(11) NOT NULL,
  `SignatureURL` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `BJCASignature` varchar(4000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CertificateNo` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `OpenID` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `IsShow` tinyint(1) NOT NULL,
  `DoctorType` int(11) NOT NULL,
  `DiseaseLabel` varchar(512) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Sort` int(11) NOT NULL,
  `RoomName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`DoctorID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for doctorschedules
-- ----------------------------
DROP TABLE IF EXISTS `doctorschedules`;
CREATE TABLE `doctorschedules`  (
  `ScheduleID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DoctorID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OPDate` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `StartTime` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `EndTime` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Number` int(11) NOT NULL,
  `AppointNumber` int(11) NOT NULL,
  `NumberSourceDetailID` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`ScheduleID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for doctorservices
-- ----------------------------
DROP TABLE IF EXISTS `doctorservices`;
CREATE TABLE `doctorservices`  (
  `ServiceID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DoctorID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ServiceType` int(11) NOT NULL,
  `ServiceSwitch` bit(1) NOT NULL,
  `ServicePrice` decimal(18, 2) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`ServiceID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for hospitaldepartments
-- ----------------------------
DROP TABLE IF EXISTS `hospitaldepartments`;
CREATE TABLE `hospitaldepartments`  (
  `DepartmentID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `HospitalID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DepartmentName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Intro` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CAT_NO` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `PARENT_CAT_NO` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`DepartmentID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for hospitals
-- ----------------------------
DROP TABLE IF EXISTS `hospitals`;
CREATE TABLE `hospitals`  (
  `HospitalID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `HospitalName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Intro` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `License` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `LogoUrl` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Address` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `AreaName` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `DrugstoreManageName` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Mobile` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Longitude` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Latitude` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `PostCode` varchar(6) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Telephone` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Email` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OrgType` int(11) NOT NULL,
  `Level` int(11) NOT NULL,
  `Path` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ParentID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ImageUrl` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ListImageUrl` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `IsShowInWeb` tinyint(1) NOT NULL,
  `IsStandalone` tinyint(1) NOT NULL,
  `IsCooperation` tinyint(1) NOT NULL,
  `IsUseWisdom` tinyint(1) NOT NULL,
  `Mp4Url` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Mp4PreviewUrl` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ChannelID` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `HomePageTheme` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ProvinceID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CityID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `AreaID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `IsUseParentOrgDrug` bit(1) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`HospitalID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for hospitalworkingtimes
-- ----------------------------
DROP TABLE IF EXISTS `hospitalworkingtimes`;
CREATE TABLE `hospitalworkingtimes`  (
  `WorkingTimeID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `HospitalID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `StartTime` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `EndTime` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Sort` int(11) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`WorkingTimeID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for ordercallbacklogs
-- ----------------------------
DROP TABLE IF EXISTS `ordercallbacklogs`;
CREATE TABLE `ordercallbacklogs`  (
  `CallbackLogID` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OrderNo` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OrgID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Status` int(11) NOT NULL,
  `TriedTimes` int(11) NOT NULL,
  `Message` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`CallbackLogID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for orderconsignees
-- ----------------------------
DROP TABLE IF EXISTS `orderconsignees`;
CREATE TABLE `orderconsignees`  (
  `OrderNo` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ConsigneeAddress` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ConsigneeName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ConsigneeTel` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `SendGoodsTime` datetime(0) NULL DEFAULT NULL,
  `IsHosAddress` decimal(18, 2) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`OrderNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for orderdetails
-- ----------------------------
DROP TABLE IF EXISTS `orderdetails`;
CREATE TABLE `orderdetails`  (
  `OrderDetailID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `OrderNO` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ProductId` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Subject` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Body` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Quantity` int(11) NOT NULL,
  `UnitPrice` decimal(18, 2) NOT NULL,
  `Discount` decimal(18, 2) NOT NULL,
  `Fee` decimal(18, 2) NOT NULL,
  `Remarks` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
  `GroupNo` int(11) NOT NULL,
  `ProductType` int(11) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`OrderDetailID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for orderdiscounts
-- ----------------------------
DROP TABLE IF EXISTS `orderdiscounts`;
CREATE TABLE `orderdiscounts`  (
  `OrderDiscountID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `OrderNo` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Privilege` int(11) NOT NULL,
  `DiscountPrice` decimal(18, 2) NOT NULL,
  `State` int(11) NOT NULL,
  `PrivilegeOutID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`OrderDiscountID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for orderlogs
-- ----------------------------
DROP TABLE IF EXISTS `orderlogs`;
CREATE TABLE `orderlogs`  (
  `OrderLogID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `OrderNo` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `OperationType` int(11) NOT NULL,
  `OperationDesc` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `OperationTime` datetime(0) NULL,
  `OperationUserID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`OrderLogID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for orderrefundlogs
-- ----------------------------
DROP TABLE IF EXISTS `orderrefundlogs`;
CREATE TABLE `orderrefundlogs`  (
  `RefundLog` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OrderNo` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `PayType` int(11) NOT NULL,
  `TradeNo` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `RefundNo` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `RefundFee` decimal(18, 2) NOT NULL,
  `RefundState` int(11) NOT NULL,
  `RefundTime` datetime(0) NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`RefundLog`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for orders
-- ----------------------------
DROP TABLE IF EXISTS `orders`;
CREATE TABLE `orders`  (
  `OrderNo` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OrderOutID` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OrderState` int(11) NOT NULL,
  `OrderType` int(11) NOT NULL,
  `CostType` int(11) NOT NULL,
  `RefundFee` decimal(18, 2) NOT NULL,
  `RefundNo` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `RefundState` int(11) NOT NULL,
  `RefundTime` datetime(0) NULL,
  `PayType` int(11) NOT NULL,
  `TradeNo` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `TradeTime` datetime(0) NULL,
  `StoreTime` datetime(0) NULL,
  `ExpressTime` datetime(0) NULL,
  `OriginalPrice` decimal(18, 2) NOT NULL,
  `totalFee` decimal(18, 2) NOT NULL,
  `OrderTime` datetime(0) NULL,
  `OrderExpireTime` datetime(0) NULL,
  `CancelTime` datetime(0) NULL,
  `FinishTime` datetime(0) NULL,
  `CancelReason` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `LogisticNo` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `LogisticState` int(11) NOT NULL,
  `LogisticCompanyName` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `LogisticWayBillNo` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `SellerID` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `OrgnazitionID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `IsEvaluated` bit(1) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`OrderNo`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for ordertradelogs
-- ----------------------------
DROP TABLE IF EXISTS `ordertradelogs`;
CREATE TABLE `ordertradelogs`  (
  `TradeLog` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OrderNo` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `PayType` int(11) NOT NULL,
  `TradeNo` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `TradeTime` datetime(0) NULL,
  `TradeFee` decimal(18, 2) NOT NULL,
  `SellerID` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
  `TradeStatus` int(11) NOT NULL,
  `OnlineTransactionNo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`TradeLog`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for regions
-- ----------------------------
DROP TABLE IF EXISTS `regions`;
CREATE TABLE `regions`  (
  `RegionID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ParentID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Name` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Level` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `HotLevel` int(11) NOT NULL,
  `Order` int(11) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`RegionID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for serviceevaluations
-- ----------------------------
DROP TABLE IF EXISTS `serviceevaluations`;
CREATE TABLE `serviceevaluations`  (
  `ServiceEvaluationID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OuterID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Score` int(11) NOT NULL,
  `EvaluationTags` varchar(512) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Content` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ProviderID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ServiceType` int(11) NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`ServiceEvaluationID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for serviceevaluationtags
-- ----------------------------
DROP TABLE IF EXISTS `serviceevaluationtags`;
CREATE TABLE `serviceevaluationtags`  (
  `ServiceEvaluationTagID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Score` int(11) NOT NULL,
  `TagName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`ServiceEvaluationTagID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for sysaccessaccounts
-- ----------------------------
DROP TABLE IF EXISTS `sysaccessaccounts`;
CREATE TABLE `sysaccessaccounts`  (
  `AccessID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `AppId` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `AppSecret` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `AppKey` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `SourceType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserKey` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`AccessID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for sysconfigs
-- ----------------------------
DROP TABLE IF EXISTS `sysconfigs`;
CREATE TABLE `sysconfigs`  (
  `ConfigKey` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ConfigValue` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Remark` varchar(512) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`ConfigKey`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sysconfigs
-- ----------------------------
INSERT INTO `sysconfigs` VALUES ('IM.AccountType', '26830', '1', NULL, '2018-05-19 15:43:25', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('IM.AdminAccount', 'administrator', '1', NULL, '2018-05-19 15:44:56', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('IM.SDKAppID', '1400092751', '1', NULL, '2018-05-19 15:44:16', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('IM.WechatAppID', 'wxc265fd9acbb92c35', '1', NULL, '2018-05-19 20:17:07', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('IM.WechatAppSec', '60cd22ce9a9f49e86013c24d801fde79', '1', NULL, '2018-05-19 20:17:38', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('Mongodb.CollectionName', 'xuhos', '1', NULL, '2018-05-19 12:54:01', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('Mongodb.ConnectionString', '\r\nmongodb://localhost', '1', NULL, '2018-05-19 12:53:12', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('MQ.HostName', '127.0.0.1', '1', NULL, '2018-05-19 14:02:31', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('MQ.Password', '123456', '1', NULL, '2018-05-19 14:03:17', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('MQ.Port', '5672', '1', NULL, '2018-05-19 14:03:42', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('MQ.UserName', 'xuhos', '1', NULL, '2018-05-19 14:02:54', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('MQ.VirtualHost', 'vh', '1', NULL, '2018-05-19 14:04:04', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('Redis.KeyPrefix', 'test', '1', NULL, '2018-05-19 12:08:19', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('Redis.ReadServerList', '127.0.0.1:16379', '1', NULL, '2018-05-19 12:07:06', NULL, NULL, NULL, NULL, b'0');
INSERT INTO `sysconfigs` VALUES ('Redis.WriteServerList', '127.0.0.1:6379', '1', NULL, '2018-05-19 12:07:41', NULL, NULL, NULL, NULL, b'0');

-- ----------------------------
-- Table structure for sysdereplications
-- ----------------------------
DROP TABLE IF EXISTS `sysdereplications`;
CREATE TABLE `sysdereplications`  (
  `SysDereplicationID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `TableName` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `OutID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DereplicationType` int(11) NOT NULL,
  `SuccessCount` int(11) NOT NULL,
  `FailCount` int(11) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`SysDereplicationID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for sysdicts
-- ----------------------------
DROP TABLE IF EXISTS `sysdicts`;
CREATE TABLE `sysdicts`  (
  `DicID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DictTypeID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DicCode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `CNName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ENName` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OrderNo` int(11) NOT NULL,
  `Remark` varchar(512) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`DicID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for syseventconsumes
-- ----------------------------
DROP TABLE IF EXISTS `syseventconsumes`;
CREATE TABLE `syseventconsumes`  (
  `ConsumeID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `EventID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `QueueName` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `RetryCount` int(11) NOT NULL,
  `Finished` tinyint(1) NOT NULL,
  PRIMARY KEY (`ConsumeID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for sysevents
-- ----------------------------
DROP TABLE IF EXISTS `sysevents`;
CREATE TABLE `sysevents`  (
  `EventID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `RouteKey` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `EventObject` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Priority` int(11) NOT NULL,
  `Enqueued` tinyint(1) NOT NULL,
  `CreateTime` datetime(0) NULL,
  PRIMARY KEY (`EventID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for sysfileindexes
-- ----------------------------
DROP TABLE IF EXISTS `sysfileindexes`;
CREATE TABLE `sysfileindexes`  (
  `MD5` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `FileUrl` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `FileType` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ReadCount` bigint(20) NOT NULL,
  `FileSize` bigint(20) NOT NULL,
  `AccessKey` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`MD5`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for sysmodules
-- ----------------------------
DROP TABLE IF EXISTS `sysmodules`;
CREATE TABLE `sysmodules`  (
  `ModuleID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ModuleName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ModuleType` int(11) NOT NULL,
  `ModuleUrl` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ParentModuleID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `TopModuleID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Level` int(11) NOT NULL,
  `Sort` int(11) NOT NULL,
  `CSSClass` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Target` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`ModuleID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for sysmonitorindexes
-- ----------------------------
DROP TABLE IF EXISTS `sysmonitorindexes`;
CREATE TABLE `sysmonitorindexes`  (
  `SysMonitorID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OutID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Category` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Value` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ModifyTime` datetime(0) NULL,
  PRIMARY KEY (`SysMonitorID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for sysshortmessagetemplates
-- ----------------------------
DROP TABLE IF EXISTS `sysshortmessagetemplates`;
CREATE TABLE `sysshortmessagetemplates`  (
  `TemplateID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `TemplateContent` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Remark` varchar(512) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`TemplateID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for userextends
-- ----------------------------
DROP TABLE IF EXISTS `userextends`;
CREATE TABLE `userextends`  (
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `LastTime` datetime(0) NULL,
  `JRegisterID` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`UserID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for userfiles
-- ----------------------------
DROP TABLE IF EXISTS `userfiles`;
CREATE TABLE `userfiles`  (
  `FileID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OutID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `FileName` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `FileUrl` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `FileType` int(11) NOT NULL,
  `Remark` varchar(512) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `AccessKey` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ResourceID` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`FileID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for userloginlogs
-- ----------------------------
DROP TABLE IF EXISTS `userloginlogs`;
CREATE TABLE `userloginlogs`  (
  `UserLoginID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `LoginAccount` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OrgID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `LoginType` int(11) NOT NULL,
  `LoginTime` datetime(0) NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`UserLoginID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for usermedicalrecords
-- ----------------------------
DROP TABLE IF EXISTS `usermedicalrecords`;
CREATE TABLE `usermedicalrecords`  (
  `UserMedicalRecordID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OPDRegisterID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `MemberID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DoctorID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Sympton` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PastMedicalHistory` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PresentHistoryIllness` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PreliminaryDiagnosis` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ConsultationSubject` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Description` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PastOperatedHistory` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FamilyMedicalHistory` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `AllergicHistory` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IndividualHistory` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `LifeHistory` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ObstetricalHistory` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `MenstrualHistory` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Advised` varchar(1000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `OrgnazitionID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`UserMedicalRecordID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for usermemberemrs
-- ----------------------------
DROP TABLE IF EXISTS `usermemberemrs`;
CREATE TABLE `usermemberemrs`  (
  `UserMemberEMRID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `MemberID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `EMRName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Date` datetime(0) NULL,
  `HospitalName` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`UserMemberEMRID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for usermembers
-- ----------------------------
DROP TABLE IF EXISTS `usermembers`;
CREATE TABLE `usermembers`  (
  `MemberID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `MemberName` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Relation` int(11) NOT NULL,
  `Gender` int(11) NOT NULL,
  `Marriage` int(11) NOT NULL,
  `Birthday` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Mobile` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `IDType` int(11) NOT NULL,
  `IDNumber` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Nationality` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Province` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ProvinceRegionID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `City` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CityRegionID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `District` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `DistrictRegionID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Town` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `TownRegionID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Village` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `VillageRegionID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `IsAllergic` tinyint(1) NOT NULL,
  `AllergicRemark` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Address` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Email` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `PostCode` varchar(6) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `IsDefault` bit(1) NOT NULL,
  `Remark` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Ethnic` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Occupation` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CompanyName` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`MemberID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for useropdregisters
-- ----------------------------
DROP TABLE IF EXISTS `useropdregisters`;
CREATE TABLE `useropdregisters`  (
  `OPDRegisterID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DoctorID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `DoctorGroupID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ScheduleID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `RegDate` datetime(0) NULL,
  `OPDDate` datetime(0) NULL,
  `OPDBeginTime` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OPDEndTime` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `ConsultContent` varchar(400) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ConsultDisease` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `OPDType` int(11) NOT NULL,
  `Fee` decimal(18, 2) NOT NULL,
  `MemberID` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `MemberName` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Gender` int(11) NOT NULL,
  `Marriage` int(11) NOT NULL,
  `Age` int(11) NOT NULL,
  `IDNumber` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `IDType` int(11) NOT NULL,
  `Mobile` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Address` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Birthday` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `OrgnazitionID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `IsUseTaskPool` tinyint(1) NOT NULL,
  `Flag` int(11) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`OPDRegisterID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for userrolemaps
-- ----------------------------
DROP TABLE IF EXISTS `userrolemaps`;
CREATE TABLE `userrolemaps`  (
  `UserRoleMapID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `RoleID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`UserRoleMapID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for userroleprevileges
-- ----------------------------
DROP TABLE IF EXISTS `userroleprevileges`;
CREATE TABLE `userroleprevileges`  (
  `PrevilegeID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `RoleID` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ModuleID` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`PrevilegeID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for userroles
-- ----------------------------
DROP TABLE IF EXISTS `userroles`;
CREATE TABLE `userroles`  (
  `RoleID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `RoleName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Memo` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `RoleType` int(11) NOT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`RoleID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserAccount` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `UserCNName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UserENName` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `UserType` int(11) NOT NULL,
  `Mobile` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Email` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `Password` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `PhotoUrl` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `RegTime` datetime(0) NULL,
  `CancelTime` datetime(0) NULL,
  `UserState` int(11) NOT NULL,
  `UserLevel` int(11) NOT NULL,
  `OrgCode` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`UserID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for usershortmessagelogs
-- ----------------------------
DROP TABLE IF EXISTS `usershortmessagelogs`;
CREATE TABLE `usershortmessagelogs`  (
  `ShortMessageLogID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `MsgLogType` int(11) NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `TelePhoneNum` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `MsgTitle` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `MsgContent` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `OutTime` datetime(0) NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`ShortMessageLogID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for userwechatmaps
-- ----------------------------
DROP TABLE IF EXISTS `userwechatmaps`;
CREATE TABLE `userwechatmaps`  (
  `UserWechatMapID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `OpenID` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `AppID` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL,
  `ModifyUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `ModifyTime` datetime(0) NULL DEFAULT NULL,
  `DeleteUserID` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `DeleteTime` datetime(0) NULL DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`UserWechatMapID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
