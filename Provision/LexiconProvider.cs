﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gramma.LanguageModel.Provision;

namespace Gramma.Lexica.Provision
{
	/// <summary>
	/// A moniker used to load a <see cref="Lexicon"/> from a binary image.
	/// </summary>
	public class LexiconProvider : LanguageFacet
	{
		#region Public properties

		/// <summary>
		/// Path of the binary image of the lexicon.
		/// </summary>
		public string Path { get; set; } = String.Empty;

		#endregion

		#region Public methods

		/// <summary>
		/// Load the <see cref="Lexicon"/> from the binary image specified in <see cref="Path"/>.
		/// </summary>
		/// <returns>Returns a <see cref="Task{Lexicon}"/> completing the action.</returns>
		public Task<Lexicon> LoadAsync()
		{
			var task = Task.Factory.StartNew(() => 
			{
				var formatter = Configuration.LexicaEnvironment.GetSerializationFormatter();

				using (var stream = new System.IO.FileStream(this.Path, System.IO.FileMode.Open))
				{
					var lexicon = (Lexicon)formatter.Deserialize(stream);

					lexicon.LanguageProvider = this.LanguageProvider;

					return lexicon;
				}
			},
			TaskCreationOptions.LongRunning);

			return task;
		}

		/// <summary>
		/// Save a <see cref="Lexicon"/> as a binary image specified by <see cref="Path"/>.
		/// </summary>
		/// <param name="lexicon">The lexicon to save.</param>
		/// <returns>Returns a task completing the operation.</returns>
		public Task SaveAsync(Lexicon lexicon)
		{
			if (lexicon == null) throw new ArgumentNullException(nameof(lexicon));

			var task = Task.Factory.StartNew(() =>
			{
				var formatter = Configuration.LexicaEnvironment.GetSerializationFormatter();

				using (var stream = new System.IO.FileStream(this.Path, System.IO.FileMode.Create))
				{
					formatter.Serialize(stream, lexicon);
				}
			},
			TaskCreationOptions.LongRunning);

			return task;
		}

		#endregion
	}
}
