﻿class TestClient
{
    public void Connected(string fullName, int id)
    {
        System.Console.WriteLine($"Connect: {fullName} {id}");
    }

    public void UpdateTransform(int id, float x, float y, float z)
    {
        System.Console.WriteLine($"Update: {id}: {x} {y} {z}");
    }

    public void Disconnect(int id)
    {
        System.Console.WriteLine($"Disconnect: {id}");
    }
}

