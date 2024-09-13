import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import LookupBroker from "../../brokers/apiBroker.lookups";
import { Lookup } from "../../models/lookups/lookup";

export const lookupService = {
    useCreatelookup: () => {
        const broker = new LookupBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((lookup: Lookup) => {
            const date = new Date();
            lookup.createdDate = lookup.updatedDate = date;
            lookup.createdBy = lookup.updatedBy = msal.accounts[0].username;

            return broker.PostLookupAsync(lookup);
        },
            {
                onSuccess: (variables: Lookup) => {
                    queryClient.invalidateQueries("LookupGetAll");
                    queryClient.invalidateQueries(["LookGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllLookups: (query: string) => {
        const broker = new LookupBroker();

        return useQuery(
            ["LookupGetAll", { query: query }],
            () => broker.GetAllLookupsAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllLookupPages: (query: string) => {
        const broker = new LookupBroker();

        return useInfiniteQuery(
            ["LookupGetAll", { query: query }],
            ({ pageParam }: { pageParam?: string }) => {
                if (!pageParam) {
                    return broker.GetLookupFirstPagesAsync(query)
                }
                return broker.GetLookupSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage: { nextPage?: string }) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyLookup: () => {
        const broker = new LookupBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((lookup: Lookup) => {
            const date = new Date();
            lookup.updatedDate = date;
            lookup.updatedBy = msal.accounts[0].username;

            return broker.PutLookupAsync(lookup);
        },
            {
                onSuccess: (data: Lookup) => {
                    queryClient.invalidateQueries("LookupGetAll");
                    queryClient.invalidateQueries(["LookupGetById", { id: data.id }]);
                }
            });
    },

    useRemoveLookup: () => {
        const broker = new LookupBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteLookupByIdAsync(id);
        },
            {
                onSuccess: (data: { id: Guid }) => {
                    queryClient.invalidateQueries("LookupGetAll");
                    queryClient.invalidateQueries(["LookupGetById", { id: data.id }]);
                }
            });
    },
}