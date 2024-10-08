import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import { Lookup } from "../../models/lookups/lookup";
import UserAccessBroker from "../../brokers/apiBroker.userAccess";
import { UserAccess } from "../../models/userAccess/userAccess";

export const userAccessService = {
    useCreateUserAccess: () => {
        const broker = new UserAccessBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((userAccess: UserAccess) => {
            const date = new Date();
            userAccess.createdDate = userAccess.updatedDate = date;
            userAccess.createdBy = userAccess.updatedBy = msal.accounts[0].username;

            return broker.PostUserAccessAsync(userAccess);
        },
            {
                onSuccess: (variables: UserAccess) => {
                    queryClient.invalidateQueries("UserAccessGetAll");
                    queryClient.invalidateQueries(["UserAccessGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllUserAccess: (query: string) => {
        const broker = new UserAccessBroker();

        return useQuery(
            ["UserAccessGetAll", { query: query }],
            () => broker.GetAllUserAccessAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllUserAccessPages: (query: string) => {
        const broker = new UserAccessBroker();

        return useInfiniteQuery(
            ["UserAccessGetAll", { query: query }],
            ({ pageParam }: { pageParam?: string }) => {
                if (!pageParam) {
                    return broker.GetUserAccessFirstPagesAsync(query)
                }
                return broker.GetUserAccessSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage: { nextPage?: string }) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyUserAccess: () => {
        const broker = new UserAccessBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((userAccess: UserAccess) => {
            const date = new Date();
            userAccess.updatedDate = date;
            userAccess.updatedBy = msal.accounts[0].username;

            return broker.PutUserAccessAsync(userAccess);
        },
            {
                onSuccess: (data: Lookup) => {
                    queryClient.invalidateQueries("UserAccessGetAll");
                    queryClient.invalidateQueries(["UserAccessGetById", { id: data.id }]);
                }
            });
    },

    useRemoveUserAccess: () => {
        const broker = new UserAccessBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteUserAccessByIdAsync(id);
        },
            {
                onSuccess: (data: { id: Guid }) => {
                    queryClient.invalidateQueries("UserAccessGetAll");
                    queryClient.invalidateQueries(["UserAccessGetById", { id: data.id }]);
                }
            });
    },
}