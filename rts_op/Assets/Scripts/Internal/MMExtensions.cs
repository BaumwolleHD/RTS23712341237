using System.Collections.Generic;

static class MMExtensions
{
    public static MapMagic.Chunk FindByCoord(this MapMagic.MapMagic MM, int x, int z)
    {
        foreach (MapMagic.Chunk chunk in MM.chunks.All())
        {
            if (chunk.coord.x == x && chunk.coord.z == z) return chunk;
        }
        return null;
    }
}