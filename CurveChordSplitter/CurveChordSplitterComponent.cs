using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Geometry;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Types.Transforms;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using CurveChordSplitter.Properties;
//using System.Linq;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace CurveChordSplitter
{
    public class CurveChordSplitterComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CurveChordSplitterComponent()
          : base("CurveChordSplitter", "CurveChordSplitter",
              "Curve Split By Chords. Returns points and parameters",
              "Curve", "Analysis")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "curve to be split", GH_ParamAccess.item);
            pManager.AddNumberParameter("Chords", "C", "Chords to split by", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "P", GH_ParamAccess.list);
            pManager.AddNumberParameter("Parameters", "T", "T", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = Curve.CreateControlPointCurve(new List<Point3d>(), 0);
            List<double> chords = new List<double>();
                
            if (!DA.GetData(0, ref curve)) return;
            if (!DA.GetDataList(1, chords)) return;

            //var kkk = GH_Point.;
            var outputPoints = new List<Point3d>();
            var outputParameters = new List<double>();
            GetPoints(curve, ref chords, ref outputPoints, ref outputParameters);


            DA.SetDataList(0, outputPoints);
            DA.SetDataList(1, outputParameters);
        }

        private void GetPoints(Curve curve, ref List<double> radiuses, ref List<Point3d> output, ref List<double> outputParameters)
        {
            //add the begining of the curve as output point
            output.Add(curve.PointAtStart);
            //create circle at start of the curve with the first available radius
            if (radiuses.Count > 0)
            {
                var circle = new Circle(curve.PointAtStart, radiuses.ElementAt<double>(0));
                radiuses.RemoveAt(0);
                //create temp curcle var of rhino type because intersection does not know how to use circle
                Curve circleAsGHCurve = null;
                GH_Convert.ToCurve(new GH_Circle(circle), ref circleAsGHCurve, GH_Conversion.Both);
                //intersect circle with curve
                var intersection = Rhino.Geometry.Intersect.Intersection.CurveCurve(circleAsGHCurve, curve, 0.0, 0.0);

                if (intersection.Count > 0)
                {
                    //create split parameter from the point
                    var splitPointParameter = intersection[0].ParameterB;
                    outputParameters.Add(splitPointParameter);
                    //split the curve on the first point
                    var splitCurvesArray = curve.Split(splitPointParameter);

                    GetPoints(splitCurvesArray[1], ref radiuses, ref output, ref outputParameters);
                }
            }
            return;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //var icon = new Bitmap("CurveChordSplitter");
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;

                return Resources.CurveChordSplitterIcon; 
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("4b0f66d9-3119-4f19-93f8-c2b894e2f840"); }
        }
    }
}
