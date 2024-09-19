import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import DelegatedAccessBroker from "../../brokers/apiBroker.delegatedAccess";
import { DelegatedAccess } from "../../models/delegatedAccess/delegatedAccess";

export const delegatedAccessService = {
    useCreatedelegatedAccess: () => {
        const broker = new DelegatedAccessBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((delegatedAccess: DelegatedAccess) => {
            const date = new Date();
            delegatedAccess.createdDate = v.updatedDate = date;
            delegatedAccess.createdBy = delegatedAccess.updatedBy = msal.accounts[0].username;

            return broker.PostDelegatedAccessAsync(delegatedAccess);
        },
            {
                onSuccess: (variables: DelegatedAccess) => {
                    queryClient.invalidateQueries("DelegatedAccessGetAll");
                    queryClient.invalidateQueries(["DelgatedAccessGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllDelegatedAccess: (query: string) => {
        const broker = new DelegatedAccessBroker();

        return useQuery(
            ["DelegatedAccessGetAll", { query: query }],
            () => broker.GetAllDelegatedAccessAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllDelegatedAccessPages: (query: string) => {
        const broker = new DelegatedAccessBroker();

        return useInfiniteQuery(
            ["DelegatedAccessGetAll", { query: query }],
            ({ pageParam }: { pageParam?: string }) => {
                if (!pageParam) {
                    return broker.GetDelegatedAccessFirstPagesAsync(query)
                }
                return broker.GetDelegatedAccessSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage: { nextPage?: string }) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyDelegatedAccess: () => {
        const broker = new DelegatedAccessBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((delegatedAccess: DelegatedAccess) => {
            const date = new Date();
            delegatedAccess.updatedDate = date;
            delegatedAccess.updatedBy = msal.accounts[0].username;

            return broker.PutDelegatedAccessAsync(delegatedAccess);
        },
            {
                onSuccess: (data: DelegatedAccess) => {
                    queryClient.invalidateQueries("DelegatedAccessGetAll");
                    queryClient.invalidateQueries(["DelegatedAccessGetById", { id: data.id }]);
                }
            });
    },

    useRemoveDelegatedAccess: () => {
        const broker = new DelegatedAccessBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteDelegatedAccessByIdAsync(id);
        },
            {
                onSuccess: (data: { id: Guid }) => {
                    queryClient.invalidateQueries("DelegatedAccessGetAll");
                    queryClient.invalidateQueries(["DelegatedAccessGetById", { id: data.id }]);
                }
            });
    },
}