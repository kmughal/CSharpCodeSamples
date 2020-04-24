namespace CSharp_Practice {

    using System;

    class Program {
        static void Main (string[] args) {
            ParseString (null)
                .Match<ResultResponse> (v =>
                    new ResultResponse {
                        StatusCode = 200,
                            Value = v.Value
                    },
                    r => new ResultResponse {
                        StatusCode = 400,
                            Value = r.Value

                    })
                .Map (v => $"{v.Value} :{v.StatusCode}")
                .Map (Console.WriteLine);

            Func<string, string, Func<string, string>> mz = GreetWith;
            Console.WriteLine (mz.Apply ("KHURRAM") ("SHAHZAD").Apply ("MUGHAL"));

            Func<CalculateCharge, Func<ValidateCharge, CalculateCharge>> payCharges = ConsolidateCharges;
            payCharges
                .Apply (CallEndPointForCalculateCharge ())
                .Apply (CallEndPointToGetValidateCharge ())
                .Map (v => $"{v.Value} = {v.ChargesPaid}")
                .Map (Console.WriteLine);
        }

        static ValidateCharge CallEndPointToGetValidateCharge () => new ValidateCharge { Value = "Validate Charge", ChargesPaid = true };
        static Func<ValidateCharge, CalculateCharge> ConsolidateCharges (CalculateCharge c) => v => new CalculateCharge { Value = v.Value, ChargesPaid = v.ChargesPaid };

        static CalculateCharge CallEndPointForCalculateCharge () => new CalculateCharge { Value = "calculate charge" };

        static Func<string, string> GreetWith (string name, string lastname) => message => $"{name}-{lastname} {message}";

        static string Test (string x, string y) {
            return x + y;
        }

        static int GetNumbers (int i) {
            return i++;

        }

        static int GetSquare (int i) {

            return (i * i);
        }

        static Union<ResultFound, ResultNotFound> ParseString (string value) {
            if (string.IsNullOrWhiteSpace (value)) {
                // Do an ajax call...
                return new ResultFound ();
            }
            return new ResultNotFound ();
        }

    }

    public class CalculateCharge {
        public string Value;
        public bool ChargesPaid;
    }

    public class ValidateCharge {
        public string Value;
        public bool ChargesPaid;
    }

    public static class UnionExtensions {
        public static TResult Map<TSource, TResult> (this TSource source, Func<TSource, TResult> func) => func (source);
        public static void Map<TSource> (this TSource source, Action<TSource> action) {
            action (source);
        }
        public static Func<T2, R> Apply<T1, T2, R> (this Func<T1, T2, R> func, T1 t1) => t2 => func (t1, t2);
        public static R Apply<T1, R> (this Func<T1, R> func, T1 t1) => func (t1);

    }

    public class ResultResponse {
        public int StatusCode;
        public string Value;
    }

    public class ResultFound {
        public string Value = "Found";
    }

    public class ResultNotFound {
        public string Value = "Found not found";
    }

    public class Union<T1, T2> where T1 : new ()
    where T2 : new () {
        public readonly T1 Instance1;
        public readonly bool Instance1Avaiable;

        public readonly T2 Instance2;
        public readonly bool Instance2Avaiable;

        public Union (T1 instance1) {
            Instance1 = instance1;
            Instance1Avaiable = true;
            Instance2Avaiable = false;
        }

        public Union (T2 instance2) {
            Instance2 = instance2;
            Instance2Avaiable = true;
            Instance1Avaiable = false;
        }

        public TResult Match<TResult> (Func<T1, TResult> func1, Func<T2, TResult> func2) {
            if (Instance1Avaiable) return func1 (Instance1);
            return func2 (Instance2);
        }

        public static implicit operator Union<T1, T2> (T1 instance) => new Union<T1, T2> (instance);

        public static implicit operator Union<T1, T2> (T2 instance) => new Union<T1, T2> (instance);
    }
}