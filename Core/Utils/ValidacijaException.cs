using System;

namespace Core.Utils;

public class ValidacijaException : Exception {
    public ValidacijaException(string message) : base(message) { }
}
