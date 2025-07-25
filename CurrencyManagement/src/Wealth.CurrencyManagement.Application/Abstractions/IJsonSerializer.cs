﻿namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface IJsonSerializer
{
    string Serialize(object? value);
    string SerializeIndented(object? value);
    object? Deserialize(string data, Type type);
    T? Deserialize<T>(string data);
}
