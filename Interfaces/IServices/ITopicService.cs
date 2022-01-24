using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.DisplayDto;
using Kiwi_review.Models.UpdateDto;

namespace Kiwi_review.Interfaces.IServices
{
    public interface ITopicService
    {
        List<TopicShowModel?>? GetAll(int movieId, string? token);
        bool Add(TopicAddingModel topic, string? token);
        bool Delete(int topicId, string token);
        bool Update(TopicUpdateModel topic, string? token);
    }
};