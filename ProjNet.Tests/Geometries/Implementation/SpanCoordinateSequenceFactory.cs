using System;
using NetTopologySuite.Geometries;

namespace ProjNET.Tests.Geometries.Implementation
{
    public class SpanCoordinateSequenceFactory : CoordinateSequenceFactory
    {
        public static CoordinateSequenceFactory Instance { get; } = new SpanCoordinateSequenceFactory();

        private SpanCoordinateSequenceFactory() : base(Ordinates.AllOrdinates)
        {}

        public override CoordinateSequence Create(Coordinate[] coordinates)
        {
            if (coordinates == null)
                return new SpanCoordinateSequence(2, 0, new double[0]);

            int dimension = 2;
            int measures = 0;
            if (coordinates[0] is CoordinateZ) dimension++;
            if (coordinates[0] is CoordinateM || coordinates[0] is CoordinateZM)
            {
                dimension++;
                measures++;
            }

            var res = Create(coordinates.Length, dimension, measures);
            for (int i = 0; i < coordinates.Length; i++)
            {
                res.SetOrdinate(i, Ordinate.X, coordinates[i].X);
                res.SetOrdinate(i, Ordinate.Y, coordinates[i].Y);
                if (res.HasZ) res.SetOrdinate(i, Ordinate.Z, coordinates[i].Z);
                if (res.HasM) res.SetOrdinate(i, Ordinate.M, coordinates[i].M);
            }

            return res;
        }

        public override CoordinateSequence Create(CoordinateSequence coordSeq)
        {
            if (coordSeq is SpanCoordinateSequence scs)
                return scs.Copy();
            if (coordSeq == null)
                return new SpanCoordinateSequence(2, 0, new double[2]);

            int dimension = coordSeq.Dimension;
            int measures = coordSeq.Measures;

            var res = Create(coordSeq.Count, dimension, measures);
            for (int i = 0; i < coordSeq.Count; i++)
            for (int j = 0; j < dimension; j++)
                res.SetOrdinate(i, (Ordinate) j, coordSeq.GetOrdinate(i, (Ordinate) j));

            return res;
        }

        public override CoordinateSequence Create(int size, int dimension, int measures)
        {
            double[] ordinateValues = new double[dimension * size];
            return new SpanCoordinateSequence(dimension, measures, ordinateValues);
        }

        public override CoordinateSequence Create(int size, Ordinates ordinates)
        {
            int dimension = OrdinatesUtility.OrdinatesToDimension(ordinates);
            return Create(size, dimension);

        }
    }
}
