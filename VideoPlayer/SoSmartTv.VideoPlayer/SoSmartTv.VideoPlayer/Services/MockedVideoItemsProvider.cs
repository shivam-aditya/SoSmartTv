﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoSmartTv.TheMovieDatabaseApi;
using SoSmartTv.VideoPlayer.ViewModels;

namespace SoSmartTv.VideoPlayer.Services
{
	public class MockedVideoItemsProvider : IVideoItemsProvider
	{
		private readonly IMovieDatabaseApi _movieDatabaseApi;

		public MockedVideoItemsProvider(IMovieDatabaseApi movideDatabaseApi)
		{
			_movieDatabaseApi = movideDatabaseApi;
		}

		private async Task<IVideoItem> FetchVideoDetails(string title)
		{
			var searchResult = (await _movieDatabaseApi.SearchVideo(title)).Results.FirstOrDefault();
			if (searchResult == null)
				return null;
			return new VideoItem(searchResult.Id, searchResult.Title, null, searchResult.GenreIds.FirstOrDefault().ToString(), searchResult.Overview, searchResult.PosterPath, searchResult.BackdropPath);
		}

		private async Task PopulateVideoDetails(IEnumerable<string> titles)
		{
			_videoItems = new List<IVideoItem>();
			var tasks = titles.Select(async x => await FetchVideoDetails(x)).ToList();
			_videoItems = (await Task.WhenAll(tasks)).Where(x => x != null).ToList();
		}

		private IList<IVideoItem> _videoItems;

		public async Task<IList<IVideoItem>> GetVideoItems()
		{

			if (_videoItems == null)
				await PopulateVideoDetails(new List<string>
				{
					"Dark Knight Rises",
					"Dark Knight",
					"Avengers",
					"Avengers Age Of Ultron",
					"King Kong",
					"Matrix",
					"Deadpool",
				});
			return _videoItems;
		}

		public async Task<IVideoItemDetails> GetVideoItem(int id)
		{
			var details = await _movieDatabaseApi.GetVideoDetails(id);
			if (details == null)
				return null;
			return new VideoItemDetails(details.Id, details.Title, null, details.Genres.FirstOrDefault().ToString(), details.Overview, details.PosterPath, details.BackdropPath);
		}
	}
}