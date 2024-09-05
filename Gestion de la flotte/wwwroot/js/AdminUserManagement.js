document.addEventListener('DOMContentLoaded', function () {
    // Configuration de DataTables
    $('#fleet-table').DataTable({
        // Configurer DataTables avec des options spécifiques
        initComplete: function () {
            // Ajouter des champs de recherche pour chaque colonne
            this.api().columns().every(function () {
                var column = this;
                var header = $(column.header());

                // Ajouter un champ de recherche pour chaque colonne
                var input = $('<input type="text" class="form-control form-control-sm" placeholder="Rechercher">')
                    .appendTo($(header).empty())
                    .on('keyup change', function () {
                        var val = $(this).val();
                        column.search(val ? val : '', false, false).draw();
                    });
            });
        },
        // Options additionnelles
        pageLength: 10,
        lengthMenu: [10, 25, 50, 75, 100],
        order: [[0, 'asc']], // Tri initial par la première colonne
        language: {
            search: "Rechercher:",
            paginate: {
                previous: "Précédent",
                next: "Suivant"
            }
        }
    });
});
