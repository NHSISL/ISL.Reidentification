/* eslint-disable @typescript-eslint/no-explicit-any */
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import './App.css';
import Root from './components/root';
import ErrorPage from './errors/error';
import { Page1 } from './pages/page1';
import { Page2 } from './pages/page2';
import { MsalProvider } from '@azure/msal-react';
import { SecuredRoute } from './components/securedRoutes';
import securityPoints from './securityMatrix';
import { Page3 } from './pages/page3';
import { QueryClientProvider } from '@tanstack/react-query';
import { queryClientGlobalOptions } from './brokers/apiBroker.globals';

// TODO:
//      - /////User Profile Screen
//      - ////Secured Routes (block / allow)
//      - Secured UI Elements (hide / show)
//      - React Query 
//      - API Secured Routes
//      - ////Unsecured routes
//      - Features

function App({ instance }: any) {

    const router = createBrowserRouter([
        {
            path: "/",
            element: <Root />,
            errorElement: <ErrorPage />,
            children: [
                {
                    path: "page1/:id",
                    element: <Page1 />
                },
                {
                    path: "page2",
                    element: <SecuredRoute allowedRoles={securityPoints.page2.view}><Page2 /></SecuredRoute>
                },
                {
                    path: "page3",
                    element: <Page3 />
                },
            ]
        }
    ]);

    return (
        <>
            <MsalProvider instance={instance}>
                <QueryClientProvider client={queryClientGlobalOptions}>
                    <RouterProvider router={router} />
                </QueryClientProvider>
            </MsalProvider>
        </>
    );


}

export default App;