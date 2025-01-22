using Todo.Core.DTOs.ListDTOs;
using Todo.Core.Entities;

namespace Todo.Api.Helpers;

public static class ListsHelper
{
  public static ListsDto MapToListsDto(TaskList list)
  {
    return new ListsDto
    {
      Id = list.Id,
      Name = list.Name,
      Description = list.Description,
    };
  }

  public static IEnumerable<ListsDto> MapToListsDto(IEnumerable<TaskList> lists) =>
    lists.Select(MapToListsDto);
}