using System;
using System.Collections.Generic;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Utility
{
    public enum OperationResultTypes
    {
        Success,
        Error
    }

    public class OperationResult
    {
        public OperationResultTypes Type { get; set; }
        public MessageNumber Message { get; set; }
        public object ReturnObject { get; set; }
        public Exception Ex { get; set; }

        public bool IsSuccess
        {
            get
            {
                return Type == OperationResultTypes.Success;
            }
        }



        public static OperationResult Build(OperationResultTypes type, object obj, Exception ex, MessageNumber message)
        {
            return new OperationResult()
            {
                Type = type,
                ReturnObject = obj,
                Message = message

            };
        }


        #region Success
        public static OperationResult Success()
        {
            return OperationResult.Build(OperationResultTypes.Success, null, null, MessageNumber.Bos);
        }
        public static OperationResult Success(object obj)
        {
            return OperationResult.Build(OperationResultTypes.Success, obj, null, MessageNumber.Bos);
        }

        public static OperationResult Success(object obj, MessageNumber message)
        {
            return OperationResult.Build(OperationResultTypes.Success, obj, null, message);
        }
        #endregion




        #region Error
        public static OperationResult Error()
        {
            return OperationResult.Build(OperationResultTypes.Error, null, null, MessageNumber.Bos);
        }
        public static OperationResult Error(object obj)
        {
            return OperationResult.Build(OperationResultTypes.Error, obj, null, MessageNumber.Bos);
        }
        public static OperationResult Error(object obj, MessageNumber message)
        {
            return OperationResult.Build(OperationResultTypes.Error, obj, null, message);
        }

        #endregion
    }
}
