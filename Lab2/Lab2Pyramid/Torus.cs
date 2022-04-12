using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2Pyramid
{
    class Torus
	{

		public List<float> Vertecies = new List<float>();
		public List<float> Normals = new List<float>();
		public List<float> TexCoords = new List<float>();

		public List<int> Indices = new List<int>();
		public List<int> lineIndices = new List<int>();

		int mainSegments; int tubeSegments; float mainRadius; float tubeRadius;
		bool withPositions = true; bool withTextureCoordinates = true; bool withNormals = true;
		float coord_X, coord_Y, coord_Z;
		public Torus(int mainSegments, int tubeSegments, float mainRadius, float tubeRadius, float coord_X = 0.0f, float coord_Y = 0.0f, float coord_Z = 0.0f,
		bool withPositions = true, bool withTextureCoordinates = true, bool withNormals = true)
		{
			this.mainSegments = mainSegments;
			this.tubeRadius = tubeRadius;
			this.tubeSegments = tubeSegments;
			this.mainRadius = mainRadius;
			this.withPositions = withPositions;
			this.withTextureCoordinates = withTextureCoordinates;
			this.withNormals = withNormals;
			this.coord_X = coord_X;
			this.coord_Y = coord_Y;
			this.coord_Z = coord_Z;

			this.GetVertices();
			this.GetNormals();
			this.GetTexCoords();
		}

		public float[] GetVertices()
		{
			float mainSegmentAngleStep = (float)MathHelper.DegreesToRadians(360.0f / (float)(mainSegments));
			float tubeSegmentAngleStep = (float)MathHelper.DegreesToRadians(360.0f / (float)(tubeSegments));

			//if (hasPositions())
			//{
			float currentMainSegmentAngle = 0.0f;
			for (int i = 0; i <= mainSegments; i++)
			{
				// Calculate sine and cosine of main segment angle
				float sinMainSegment = (float)Math.Sin(currentMainSegmentAngle);
				float cosMainSegment = (float)Math.Cos(currentMainSegmentAngle);
				float currentTubeSegmentAngle = 0.0f;
				for (int j = 0; j <= tubeSegments; j++)
				{
					// Calculate sine and cosine of tube segment angle
					float sinTubeSegment = (float)Math.Sin(currentTubeSegmentAngle);
					float cosTubeSegment = (float)Math.Cos(currentTubeSegmentAngle);

					// Calculate vertex position on the surface of torus
					Vertecies.Add(coord_X + (mainRadius + tubeRadius * cosTubeSegment) * cosMainSegment); // x //coord_x + лишнее
					Vertecies.Add(coord_Y + (mainRadius + tubeRadius * cosTubeSegment) * sinMainSegment); // y
					Vertecies.Add(coord_Z + tubeRadius * sinTubeSegment);//z
															   //auto surfacePosition = glm::vec3(
															   //	(_mainRadius + _tubeRadius * cosTubeSegment) * cosMainSegment,
															   //	(_mainRadius + _tubeRadius * cosTubeSegment) * sinMainSegment,
															   //	_tubeRadius * sinTubeSegment);

					//_vbo.addData(&surfacePosition, sizeof(glm::vec3));

					// Update current tube angle
					currentTubeSegmentAngle += tubeSegmentAngleStep;
				}

				// Update main segment angle
				currentMainSegmentAngle += mainSegmentAngleStep;
				//}
			}




			return Vertecies.ToArray();
		}

		public float[] GetNormals()
		{
			var mainSegmentAngleStep = (float)MathHelper.DegreesToRadians(360.0f / (float)(mainSegments));
			var tubeSegmentAngleStep = (float)MathHelper.DegreesToRadians(360.0f / (float)(tubeSegments));

			float currentMainSegmentAngle = 0.0f;
			for (int i = 0; i <= mainSegments; i++)
			{
				// Calculate sine and cosine of main segment angle
				float sinMainSegment = (float)Math.Sin(currentMainSegmentAngle);
				float cosMainSegment = (float)Math.Cos(currentMainSegmentAngle);
				float currentTubeSegmentAngle = 0.0f;
				for (int j = 0; j <= tubeSegments; j++)
				{
					// Calculate sine and cosine of tube segment angle
					float sinTubeSegment = (float)Math.Sin(currentTubeSegmentAngle);
					float cosTubeSegment = (float)Math.Cos(currentTubeSegmentAngle);

					Normals.Add(coord_X + cosMainSegment * cosTubeSegment); //x 
					Normals.Add(coord_Y + sinMainSegment * cosTubeSegment); //y
					Normals.Add(coord_Z + sinTubeSegment); //z

					// Update current tube angle
					currentTubeSegmentAngle += tubeSegmentAngleStep;
				}

				// Update main segment angle
				currentMainSegmentAngle += mainSegmentAngleStep;
			}
			return Normals.ToArray();
		}

		public float[] GetTexCoords()
		{
			float mainSegmentTextureStep = 2.0f / (float)(mainSegments);
			float tubeSegmentTextureStep = 1.0f / (float)(tubeSegments);

			float currentMainSegmentTexCoordV = 0.0f;
			for (int i = 0; i <= mainSegments; i++)
			{
				float currentTubeSegmentTexCoordU = 0.0f;
				for (int j = 0; j <= tubeSegments; j++)
				{

					TexCoords.Add(currentTubeSegmentTexCoordU);
					TexCoords.Add(currentMainSegmentTexCoordV);

					currentTubeSegmentTexCoordU += tubeSegmentTextureStep;
				}

				// Update texture coordinate of main segment
				currentMainSegmentTexCoordV += mainSegmentTextureStep;
			}

			return TexCoords.ToArray();
		}

		public int[] GetIndices()
		{
			int currentVertexOffset = 0;
			for (int i = 0; i < mainSegments; i++)
			{
				for (int j = 0; j <= tubeSegments; j++)
				{
					int vertexIndexA = currentVertexOffset;
					Indices.Add(vertexIndexA);
					int vertexIndexB = currentVertexOffset + tubeSegments + 1;
					Indices.Add(vertexIndexB);
					currentVertexOffset++;
				}


			}


			return Indices.ToArray();
		}

		public float[] GetAllTogether()
		{
			List<float> result = new List<float>();

			for (int i = 0; i < Vertecies.Count / 3; ++i)
			{
				result.Add(Vertecies[i * 3]);
				result.Add(Vertecies[i * 3 + 1]);
				result.Add(Vertecies[i * 3 + 2]);

				result.Add(Normals[i * 3]);
				result.Add(Normals[i * 3 + 1]);
				result.Add(Normals[i * 3 + 2]);

				result.Add(TexCoords[i * 2]);
				result.Add(TexCoords[i * 2 + 1]);
			}


			return result.ToArray();


		}
	}
}
