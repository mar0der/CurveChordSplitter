using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace CurveChordSplitter
{
    public class CurveChordSplitterInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Curve Split By Chords";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                // var icon = new Bitmap("CurveChordSplitter");
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Curve Split By Chords. Returns points and parameters";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("cfc0a892-c2a5-46c5-a65f-b071f00f438c");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Petar Petkov";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "petar.b.petkov@gmail.com";
            }
        }
    }
}
