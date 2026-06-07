using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoraEnterprise.SharedKernel.Common.Models
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }

        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && error != string.Empty)
                throw new InvalidOperationException();

            if (!isSuccess && string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, string.Empty);

        public static Result Failure(string error) => new(false, error);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value)
            => new(value, true, string.Empty);

        public static Result<T> Failure(string error)
            => new(default!, false, error);
    }
}
