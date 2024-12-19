import React, { useEffect, useState } from "react";
import {
  fetchTop10Movies,
  fetchTop10TVShows,
  fetchTop10Actors,
  fetchImages,
} from "../services/apiService";
import Carousel from "../components/Top10Carousel";
import "./HomePage.css";

const HomePage = () => {
  const [top10Movies, setTop10Movies] = useState([]);
  const [top10TVShows, setTop10TVShows] = useState([]);
  const [top10Actors, setTop10Actors] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [movies, tvShows, actors] = await Promise.all([
          fetchTop10Movies(),
          fetchTop10TVShows(),
          fetchTop10Actors(),
        ]);

        const actorsWithImages = await Promise.all(
          actors.map(async (actor) => {
            const image = await fetchImages(actor.nConst);
            return { ...actor, image };
          })
        );

        setTop10Movies(movies);
        setTop10TVShows(tvShows);
        setTop10Actors(actorsWithImages);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

  return (
    <div className="homepage">
      <Carousel title="Popular Movies.." items={top10Movies} itemType="title" />
      <Carousel
        title="Popular TV Shows.."
        items={top10TVShows}
        itemType="title"
      />
      <Carousel title="Popular Actors.." items={top10Actors} itemType="name" />
    </div>
  );
};

export default HomePage;
