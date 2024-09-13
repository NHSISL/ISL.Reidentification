import { useEffect, useState } from "react";
import { lookupService } from "../../foundations/lookupService";
import { LookupView } from "../../../models/views/components/lookups/lookupView";
import { Guid } from "guid-typescript";

type LookupViewServiceResponse = {
    mappedLookUps: LookupView[] | undefined;
    pages: Array<{ data: LookupView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: LookupView[] }> } | undefined;
    refetch: () => void;
};

export const lookupViewService = {
    useCreateLookup: () => {
        return lookupService.useCreatelookup();
    },

    useGetAllLookups: (searchTerm?: string): LookupViewServiceResponse => {
        let query = `?$orderby=createdDate desc`;

        if (searchTerm) {
            query = query + `&$filter=contains(Name,'${searchTerm}')`;
        }

        const response = lookupService.useRetrieveAllLookupPages(query);
        const [mappedLookUps, setMappedLookups] = useState<Array<LookupView>>();
        const [pages, setPages] = useState<Array<{ data: LookupView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const lookups: Array<LookupView> = [];
                response.data.pages.forEach((x: { data: LookupView[] }) => {
                    x.data.forEach((lookup: LookupView) => {
                        lookups.push(new LookupView(
                            lookup.id,
                            lookup.name,
                            lookup.value,
                            lookup.createdBy,
                            lookup.createdDate,
                            lookup.updatedBy,
                            lookup.updatedDate,
                        ));
                    });
                });

                setMappedLookups(lookups);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedLookUps,
            pages,
            isLoading: response.isLoading,
            fetchNextPage: response.fetchNextPage,
            isFetchingNextPage: response.isFetchingNextPage,
            hasNextPage: !!response.hasNextPage,
            data: response.data,
            refetch: response.refetch
        };
    },

    useGetLookupById: (id: Guid) => {
        const query = `?$filter=id eq ${id}`;
        const response = lookupService.useRetrieveAllLookupPages(query);
        const [mappedLookup, setMappedLookup] = useState<LookupView>();

        useEffect(() => {
            if (response.data && response.data.pages && response.data.pages[0].data[0]) {
                const lookup = response.data.pages[0].data[0];
                const lookupView = new LookupView(
                    lookup.id,
                    lookup.name,
                    lookup.value,
                    lookup.createdBy,
                    lookup.createdDate,
                    lookup.updatedBy,
                    lookup.updatedDate
                );

                setMappedLookup(lookupView);
            }
        }, [response.data]);

        return {
            mappedLookup,
            ...response
        };
    },

    useUpdateLookup: () => {
        return lookupService.useModifyLookup();
    },

    useRemoveLookup: () => {
        return lookupService.useRemoveLookup();
    },
};