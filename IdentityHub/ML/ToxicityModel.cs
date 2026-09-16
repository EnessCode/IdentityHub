using Microsoft.ML;
using Microsoft.ML.Data;

namespace IdentityHub.ML
{
	public class CommentData
	{
		[LoadColumn(0)]
		public string Text { get; set; }

		[LoadColumn(1), ColumnName("Label")]
		public bool IsToxic { get; set; }
	}

	public class CommentPrediction
	{
		[ColumnName("PredictedLabel")]
		public bool IsToxic { get; set; }

		public float Probability { get; set; }
	}

	public static class ToxicityModel
	{
		private static MLContext _mlContext = new MLContext();
		private static PredictionEngine<CommentData, CommentPrediction> _predictionEngine;

		public static void TrainModel()
		{
			var trainingData = new[]
			{
				new CommentData { Text = "Harika bir paylaşım, ellerine sağlık.", IsToxic = false },
				new CommentData { Text = "Çok faydalı bir yazı olmuş, teşekkürler.", IsToxic = false },
				new CommentData { Text = "Tebrik ederim, başarılarınızın devamını dilerim.", IsToxic = false },
				new CommentData { Text = "Emeğinize sağlık, aradığımı bu yazıda buldum.", IsToxic = false },
				new CommentData { Text = "Çok bilgilendirici ve açıklayıcı bir içerik.", IsToxic = false },
				new CommentData { Text = "Harika fikir, kesinlikle denenmeli.", IsToxic = false },
				new CommentData { Text = "Katılıyorum, çok doğru bir yaklaşım.", IsToxic = false },
				new CommentData { Text = "Detaylı anlatım için çok teşekkürler.", IsToxic = false },

				new CommentData { Text = "Bence bu kısım biraz eksik kalmış, geliştirilebilir.", IsToxic = false },
				new CommentData { Text = "Kodda ufak bir hata var sanırım ama yine de teşekkürler.", IsToxic = false },
				new CommentData { Text = "Beklediğim kadar detaylı bulamadım açıkçası.", IsToxic = false },
				new CommentData { Text = "Anlatım biraz karmaşık olmuş ama fikir güzel.", IsToxic = false },

				new CommentData { Text = "Bu ne biçim bir makale, iğrenç olmuş.", IsToxic = true },
				new CommentData { Text = "Geri zekalı mısın sen, böyle yazı mı olur?", IsToxic = true },
				new CommentData { Text = "Mal mal konuşmayı kes, saçma sapan şeyler yazmışsın.", IsToxic = true },
				new CommentData { Text = "Tam bir aptalsın, hayatımda gördüğüm en kötü içerik.", IsToxic = true },
				new CommentData { Text = "Defol git buradan, rezil herif.", IsToxic = true },
				new CommentData { Text = "Sen bu işi hiç bilmiyorsun, defol git.", IsToxic = true },
				new CommentData { Text = "Salak salak yorumlar yapmayın.", IsToxic = true },
				new CommentData { Text = "Yazdığın şey tamamen çöp, zaman kaybı.", IsToxic = true },
				new CommentData { Text = "Defolun gidin buradan seni ve siteni.", IsToxic = true }
			};

			var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

			var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(CommentData.Text))
				.Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression("Label", "Features"));

			var model = pipeline.Fit(dataView);

			_predictionEngine = _mlContext.Model.CreatePredictionEngine<CommentData, CommentPrediction>(model);
		}

		public static CommentPrediction GetPrediction(string commentText)
		{
			if (_predictionEngine == null)
			{
				TrainModel();
			}

			return _predictionEngine.Predict(new CommentData { Text = commentText });
		}
	}
}