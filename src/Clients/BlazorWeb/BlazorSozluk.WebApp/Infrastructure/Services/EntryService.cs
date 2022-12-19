using BlazorSozluk.Common.Models.Page;
using BlazorSozluk.Common.Models.Queries;
using BlazorSozluk.Common.Models.RequestModels;
using BlazorSozluk.WebApp.Common;
using BlazorSozluk.WebApp.Infrastructure.Services.Interfaces;
using System.Net.Http.Json;

namespace BlazorSozluk.WebApp.Infrastructure.Services
{
	public class EntryService : IEntryService
	{
		private readonly HttpClient client;

		public EntryService(HttpClient client)
		{
			this.client = client;
		}

		public async Task<List<GetEntriesViewModel>> GetEntries()
		{

			var result = await client.GetFromJsonAsync<List<GetEntriesViewModel>>($"{HostConf.host}/api/Entry/GetEntries?TodayEntries=false&Count=30");
			return result;
		}

		public async Task<GetEntryDetailViewModel> GetEntryDetail(Guid entryId)
		{
			var result = await client.GetFromJsonAsync<GetEntryDetailViewModel>($"{HostConf.host}/api/entry/{entryId}");
			return result;
		}

		public async Task<PagedViewModel<GetEntryDetailViewModel>> GetMainPageEntries(int page, int pageSize)
		{
			var result = await client.GetFromJsonAsync<PagedViewModel<GetEntryDetailViewModel>>(
				$"{HostConf.host}/api/entry/mainpageentries?page={page}&pageSize={pageSize}");
			return result;
		}

		public async Task<PagedViewModel<GetEntryDetailViewModel>> GetProfilePageEntries(int page, int pageSize, string userName = null)
		{
			var result = await client.GetFromJsonAsync<PagedViewModel<GetEntryDetailViewModel>>(
				$"{HostConf.host}/api/entry/UserEntries?userName={userName}&page={page}&pageSize={pageSize}");
			return result;
		}
		public async Task<PagedViewModel<GetEntryCommentsViewModel>> GetEntryComments(Guid entryId, int page, int pageSize)
		{

			var result = await client.GetFromJsonAsync<PagedViewModel<GetEntryCommentsViewModel>>
				($"{HostConf.host}/api/entry/comments/{entryId}?page={page}&pageSize={pageSize}");
			return result;
		}

		public async Task<Guid> CreateEntry(CreateEntryCommand createEntryCommand)
		{
			var res = await client.PostAsJsonAsync($"{HostConf.host}/api/Entry/CreateEntry", createEntryCommand);
			if (!res.IsSuccessStatusCode)
			{
				return Guid.Empty;
			}
			var guidStr = await res.Content.ReadAsStringAsync();
			return new Guid(guidStr.Trim('"'));
		}

		public async Task<Guid> CreateEntryComment(CreateEntryCommentCommand createEntryCommentCommand)
		{

			var res = await client.PostAsJsonAsync($"{HostConf.host}/api/Entry/CreateEntryComment", createEntryCommentCommand);
			if (!res.IsSuccessStatusCode)
			{
				return Guid.Empty;
			}
			var guidStr = await res.Content.ReadAsStringAsync();
			return new Guid(guidStr.Trim('"'));
		}
		public async Task<List<SearchEntryViewModel>> SearchBySubject(string searchSubject)
		{
			var result = await client.GetFromJsonAsync<List<SearchEntryViewModel>>(
			  $"{HostConf.host}/api/Entry/Search?SearchText={searchSubject}"
				);
			return result;
		}
	}
}
