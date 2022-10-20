using System;
using PX.Data;

namespace LUMCustomizations.DAC
{
    [Serializable]
    [PXCacheName("LUMWaldomPreference")]
    public class LUMWaldomPreference : IBqlTable
    {
        #region Ftpurl
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "FTP URL")]
        public virtual string FtpHost { get; set; }
        public abstract class ftpHost : PX.Data.BQL.BqlString.Field<ftpHost> { }
        #endregion

        #region UserName
        [PXDBString(1024, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "User Name")]
        public virtual string FTPUserName { get; set; }
        public abstract class ftpUserName : PX.Data.BQL.BqlString.Field<ftpUserName> { }
        #endregion

        #region Password
        [PXRSACryptString(1024, IsUnicode = true)]
        [PXUIField(DisplayName = "Password")]
        public virtual string FTPPassword { get; set; }
        public abstract class ftpPassword : PX.Data.BQL.BqlString.Field<ftpPassword> { }
        #endregion

        #region Port
        [PXDBInt()]
        [PXUIField(DisplayName = "Port")]
        public virtual int? FTPPort { get; set; }
        public abstract class ftpPort : PX.Data.BQL.BqlInt.Field<ftpPort> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}