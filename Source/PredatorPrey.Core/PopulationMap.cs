using System.Collections;
using System.Collections.Concurrent;
using System.Drawing;

namespace PredatorPrey;

public class PopulationMap : IEnumerable<Organism>
{
    public int Population => _organismToLocation.Count;
    public ConcurrentDictionary<Organism, Point> _organismToLocation { get; } = new();
    public ConcurrentDictionary<Point, HashSet<Organism>> _locationToOrganisms { get; } = new();

    public Point? GetOrganismLocation(Organism organism)
    {
        return _organismToLocation.TryGetValue(organism, out Point location)
            ? location
            : null;
    }

    public IEnumerable<Organism> GetOrganismsAtLocation(int x, int y)
    {
        return GetOrganismsAtLocation(new Point(x, y));
    }

    public IEnumerable<Organism> GetOrganismsAtLocation(Point location)
    {
        return _locationToOrganisms.TryGetValue(location, out var organisms)
            ? organisms
            : Enumerable.Empty<Organism>();
    }

    public void Add(Organism organism, int x, int y)
    {
        Add(organism, new Point(x, y));
    }

    public void Add(Organism organism, Point location)
    {
        // Remove from previous location if exists
        if (_organismToLocation.TryRemove(organism, out Point oldLocation))
        {
            _locationToOrganisms.TryGetValue(oldLocation, out var oldLocationOrganisms);
            oldLocationOrganisms?.Remove(organism);
        }

        // Add to new location
        _organismToLocation[organism] = location;

        var locationOrganisms = _locationToOrganisms.GetOrAdd(location, _ => new HashSet<Organism>());
        locationOrganisms.Add(organism);
    }

    public void Remove(Organism organism)
    {
        if (_organismToLocation.TryRemove(organism, out Point location))
        {
            _locationToOrganisms.TryGetValue(location, out var locationOrganisms);
            locationOrganisms?.Remove(organism);
        }
    }

    public IEnumerator<Organism> GetEnumerator()
    {
        return _organismToLocation.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
