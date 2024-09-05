document.addEventListener('DOMContentLoaded', function () {
    const cards = document.querySelectorAll('.card');
    const loader = document.getElementById('page-loader');

    // Ajouter une transition fluide pour le changement de couleur de fond au survol
    cards.forEach(card => {
        card.addEventListener('mouseover', function () {
            this.style.transition = 'background-color 0.3s ease-in-out';
            this.style.backgroundColor = '#f8f9fa'; // Légère variation de couleur au survol
        });

        card.addEventListener('mouseout', function () {
            this.style.transition = 'background-color 0.3s ease-in-out';
            this.style.backgroundColor = ''; // Remet la couleur d'origine
        });

        // Clic - Affichage d'une alerte avec le titre de la carte

    });

    // Ajouter une transition pour les changements de page
    cards.forEach(card => {
        const link = card.querySelector('a');
        if (link) {
            link.addEventListener('click', function (event) {
                event.preventDefault(); // Empêche le comportement par défaut du lien

                loader.classList.remove('d-none'); // Affiche le loader
                document.body.classList.add('fade-out'); // Ajoute la classe pour la transition

                setTimeout(() => {
                    window.location.href = this.href; // Redirige vers la page après la transition
                }, 300); // Délai correspondant à la durée de la transition CSS
            });
        }
    });
});
