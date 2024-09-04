/* eslint-disable @typescript-eslint/no-explicit-any */
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import './App.css';
import Root from './components/root';
import ErrorPage from './errors/error';
import { Page1 } from './pages/page1';
import { Page2 } from './pages/page2';
import { MsalProvider } from '@azure/msal-react';

// TODO:
//      - User Profile Screen
//      - Secured Routes (block / allow)
//      - Secured UI Elements (hide / show)
//      - React Query 
//      - API Secured Routes
//      - Unsecured routes
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
                element: <Page2 />
            }
          ]
        }
      ]);

    return (
        <>
            <MsalProvider instance={instance}>
                <RouterProvider router={router} />
            </MsalProvider>
        </>
    );


}

export default App;