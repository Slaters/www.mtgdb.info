using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public interface IRepository
    {
        Guid AddPlaneswalker(string userName, string password, string email);
        void RemovePlaneswalker(Guid id);
        Profile GetProfile(Guid id);
        Profile GetProfile(string name);
        Planeswalker UpdatePlaneswalker(Planeswalker planeswalker);

        UserCard AddUserCard(Guid walkerId, int multiverseId, int amount);
        UserCard[] GetUserCards(Guid walkerId, int[] multiverseIds);
        UserCard[] GetUserCards(Guid walkerId);
        UserCard[] GetUserCards (Guid walkerId, string setId);
        Dictionary<string, int> GetSetCardCounts(Guid walkerId);

        Guid AddCardChangeRequest(CardChange card);
        CardChange[] GetCardChangeRequests(int mvid);
        CardChange[] GetChangeRequests(string status = null);
        CardChange GetCardChangeRequest(Guid id);
        CardChange UpdateCardChangeStatus(Guid id, string status, string field = null);

        Guid AddCard(NewCard card);
        NewCard GetCard(Guid id);
        NewCard [] GetNewCards(string status = null);
        NewCard UpdateNewCardStatus(Guid id, string status);

        Guid AddSet(NewSet set);
        NewSet GetSet(Guid id);
        NewSet [] GetNewSets(string status = null);
        NewSet UpdateNewSetStatus(Guid id, string status);

        Guid AddCardSetChangeRequest(SetChange change);
        SetChange GetCardSetChangeRequest(Guid id);
        SetChange[] GetCardSetChangeRequests(string setId);
        SetChange[] GetSetChangeRequests(string status = null);
        SetChange UpdateCardSetChangeStatus(Guid id, string status, string field = null);


//        void UpdatePlaneswalker(Planeswalker planeswalker);
//
//        void SetCardAmount(Guid userId, int multiverseId, int amount);
    }
}

