import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import './App.css';
import Root from './components/root';
import ErrorPage from './components/error';
import { Page1 } from './components/page1';
import { Page2 } from './components/page2';


function App() {

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
            <RouterProvider router={router} />
        </>
    );


}

export default App;