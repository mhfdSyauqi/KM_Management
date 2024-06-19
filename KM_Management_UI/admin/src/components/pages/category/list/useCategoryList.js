import { ref } from 'vue'
import { GetCategoryListAsync} from '@/api/reqCategory.js'
const category_list = ref([]);
const filter = ref({
    Uid_Reference:'',
    Layer:'',
    Is_Active:'',
  })

async function GetCategoryListByFilter() {
  const result = await GetCategoryListAsync(
    filter.value.Uid_Reference,
    filter.value.Layer,
    filter.value.Is_Active,
    )

  if (result.is_success) {
    category_list.value = result.category_list
  }
}
export { filter, category_list, GetCategoryListByFilter }
